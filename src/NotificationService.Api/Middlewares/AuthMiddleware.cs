using System.Text.Json;
using NotificationService.Domain.Entities;
using NotificationService.SharedKernel.Interfaces;
using NotificationService.Domain.Enums;
using NotificationService.Api.Attributes;
using NotificationService.Application.Contracts.Interfaces.Repositories;
using NotificationService.SharedKernel.Resources;
using NotificationService.Application.Common.Dtos;

namespace NotificationService.Api.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private IRepository<Platform> _platformRepository;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    private static bool SkipAuthorization(HttpContext context)
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
            await UnauthorizedResponse(context, Messages.ApiKeyNotProvided);
            return;
        }
        
        _platformRepository = context.RequestServices.GetRequiredService<IRepository<Platform>>();
        var platform = await _platformRepository.FindOneAsync(x => x.ApiKey == headerApiKey.ToString());

        if (platform is null)
        {
            await UnauthorizedResponse(context, Messages.ApiKeyNotValid);
            return;
        }
        
        if (!platform.IsActive)
        {
            await UnauthorizedResponse(context, Messages.ApiKeyNotActive);
            return;
        }

        var platformDto = new PlatformDto
        {
            PlatformId = platform.PlatformId,
            Name = platform.Name,
            Description = platform.Description,
            IsActive = platform.IsActive,
            IsAdmin = platform.IsAdmin,
            ApiKey = platform.ApiKey
        };
        context.Items[nameof(PlatformDto)] = platformDto;

        await _next(context);
    }

    private async Task UnauthorizedResponse(HttpContext context, string message)
    {
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";

        var finalResponse = new BaseResponse<INoDataResponse>((int) ResultCode.AccessDenied, message);

        var result = JsonSerializer.Serialize(finalResponse, 
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });
        await context.Response.WriteAsync(result);
    }

}