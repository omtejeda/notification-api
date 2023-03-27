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

namespace NotificationService.Middlewares
{
    public class OwnerGuardMiddleware
    {
        private readonly RequestDelegate _next;
        
        public OwnerGuardMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var platformNameProperty = "platformName";

            var currentPlatform = (PlatformDTO) context.Items[nameof(PlatformDTO)];
            var platformNameFromRequest = context.Request.Query[platformNameProperty].ToString() ?? context.Request.RouteValues[platformNameProperty].ToString();
            
            if (!(currentPlatform.IsAdmin ?? false) && currentPlatform.Name.ToLower() != platformNameFromRequest.ToLower())
            {
                await UnauthorizedResponse(context, $"You're trying to use a platform you're not allowed to.");
                return;
            }
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