using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Core.Platforms.Entities;
using NotificationService.Core.Common.Interfaces;
using NotificationService.Core.Common.Enums;
using NotificationService.Api.Attributes;
using NotificationService.Core.Platforms.Dtos;
using NotificationService.Contracts.Interfaces.Repositories;
using NotificationService.Contracts.ResponseDtos;

namespace NotificationService.Api.Middlewares
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
                ?.GetMetadata<AllowAnonymousAttribute>() is not null;
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

            if (platform is null || !(platform.IsActive ?? false))
            {
                await UnauthorizedResponse(context, $"Unathorized client. {(platform is null ? "It's not valid" : "It's not active")}");
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
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });
            await context.Response.WriteAsync(result);
        }

    }
}