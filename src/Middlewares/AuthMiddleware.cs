using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Repositories;
using NotificationService.Entities;
using NotificationService.Dtos;
using NotificationService.Interfaces;
using NotificationService.Enums;
using NotificationService.Attributes;

namespace NotificationService.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private IRepository<Platform> _platformRepository;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private bool SkipAuthorization(HttpContext context)
        {
            return context
                ?.GetEndpoint()
                ?.Metadata
                ?.GetMetadata<AllowAnonymousAttribute>() != null;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (SkipAuthorization(context))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue("apiKey", out var headerApiKey))
            {
                await UnauthorizedResponse(context, "Api key was not provided");
                return;
            }
            
            _platformRepository = context.RequestServices.GetRequiredService<IRepository<Platform>>();
            var platform = await _platformRepository.FindOneAsync(x => x.ApiKey == headerApiKey);

            if ( platform == null || !(platform.IsActive ?? false) )
            {
                await UnauthorizedResponse(context, $"Unathorized client. {(platform == null ? "It's not valid" : "It's not active")}");
                return;
            }

            var platformDto = new PlatformDTO
            {
                PlatformId = platform.PlatformId,
                Name = platform.Name,
                Description = platform.Description,
                IsActive = platform.IsActive,
                IsAdmin = platform.IsAdmin,
                ApiKey = platform.ApiKey
            };
            context.Items[nameof(PlatformDTO)] = platformDto;

            await _next(context);
        }

        private async Task UnauthorizedResponse(HttpContext context, string message)
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";

            var finalResponse = new FinalResponseDTO<INoDataResponse>((int) ErrorCode.AccessDenied, message);

            var result = JsonSerializer.Serialize(finalResponse, 
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    IgnoreNullValues = true
                });
            await context.Response.WriteAsync(result);
        }

    }
}