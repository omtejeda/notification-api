using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.Services;
using NotificationService.Services.Interfaces;
using Microsoft.OpenApi.Models;
using NotificationService.Repositories;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using NotificationService.Extensions;
using MongoDB.Driver;
using NotificationService.Repositories.Helpers;
using NotificationService.Middlewares;
using AutoMapper;
using System.Reflection;
using NotificationService.Utils;

namespace NotificationService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<INotificationsService, NotificationsService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ISmsService, SmsService>();
            services.AddTransient<IPlatformService, PlatformService>();
            services.AddTransient<ITemplateService, TemplateService>();
            services.AddTransient<IProviderService, ProviderService>();
            services.AddTransient<ICatalogService, CatalogService>();
            services.AddTransient<IHttpClientService, HttpClientService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddSingleton<IMongoDatabase>(s =>
                new MongoClient(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"))
                    .GetDatabase(Environment.GetEnvironmentVariable("DB_NAME"))
                    .InitializeMappings()
            );
            services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));

            services.AddRouting(o => o.LowercaseUrls = true);
            services.AddControllers(o =>
            {
                o.UseGeneralRoutePrefix(NotificationService.Utils.Routes.GlobalPrefix);
            });

            services.AddVersioning();
            services.AddSwaggerGen( c => 
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Notification API",
                    Version = "v1",
                    Contact = new OpenApiContact { Name = "Software Engineer | Omarky Tejeda", Email = "omtejeda@humano.com.do" },
                    Description = $"Environment: {SystemUtil.GetEnvironment()}"
                });

                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "ApiKey"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
                        },
                        new string[] { }
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseCors();

            app.UseAuthorization();

            app.UseMiddleware<AuthMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
