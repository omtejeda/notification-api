using System.Text.Json;
using NotificationService.Domain.Entities;
using NotificationService.SharedKernel.Interfaces;
using NotificationService.Domain.Enums;
using NotificationService.Api.Attributes;
using NotificationService.Application.Contracts.Interfaces.Repositories;
using NotificationService.SharedKernel.Resources;
using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Api.Middlewares;

/// <summary>
/// Middleware for handling API key authentication in the ASP.NET Core application.
/// This middleware checks the presence and validity of an API key in the request headers.
/// </summary>
/// <param name="next">The next middleware delegate in the pipeline.</param>
public class AuthMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    private IRepository<Platform> _platformRepository;

    /// <summary>
    /// Checks if the authorization can be skipped for the current request based on the presence of the <see cref="AllowAnonymousAttribute"/>.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    /// <returns><c>true</c> if authorization should be skipped; otherwise, <c>false</c>.</returns>
    private static bool SkipAuthorization(HttpContext context)
    {
        return context
            ?.GetEndpoint()
            ?.Metadata
            ?.GetMetadata<AllowAnonymousAttribute>() is not null;
    }

    /// <summary>
    /// Invokes the middleware to process the HTTP context, validating the API key and setting the platform information in the context items.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
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

    /// <summary>
    /// Writes a JSON response to the HTTP context indicating unauthorized access.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    /// <param name="message">A message describing the unauthorized access.</param>
    private async Task UnauthorizedResponse(HttpContext context, string message)
    {
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";

        var finalResponse = new BaseResponse<INoDataResponse>((int)ResultCode.AccessDenied, message);

        var result = JsonSerializer.Serialize(finalResponse, 
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });
        await context.Response.WriteAsync(result);
    }
}