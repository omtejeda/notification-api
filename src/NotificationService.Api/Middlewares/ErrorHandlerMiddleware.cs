using System.Text.Json;
using NotificationService.Domain.Enums;
using NotificationService.Application.Exceptions;
using NotificationService.Application.Common.Models;
using NotificationService.Api.Utils;

namespace NotificationService.Api.Middlewares;

/// <summary>
/// Middleware for handling errors in the ASP.NET Core application.
/// This middleware catches exceptions thrown during request processing and returns a standardized JSON response.
/// </summary>
/// <param name="next">The next middleware delegate in the pipeline.</param>
public class ErrorHandlerMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    /// <summary>
    /// Invokes the middleware, processing the HTTP context and handling any exceptions that occur.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (RuleValidationException ex)
        {
            await Response(context, StatusCodes.Status400BadRequest, (int)ResultCode.ValidationError, ex.Message);
        }
        catch (TimeoutException ex)
        {
            await Response(context, StatusCodes.Status408RequestTimeout, (int)ResultCode.Warning, ex.Message);
        }
        catch (Exception ex)
        {
            await Response(context, StatusCodes.Status500InternalServerError, (int)ResultCode.Error, ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }

    /// <summary>
    /// Writes a JSON response to the HTTP context based on the error details.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    /// <param name="statusCode">The HTTP status code to set for the response.</param>
    /// <param name="code">A custom error code representing the type of error.</param>
    /// <param name="message">A message describing the error.</param>
    private static async Task Response(HttpContext context, int statusCode, int code, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var detailsResponse = new BaseResponse<object>(code, message);

        var result = JsonUtils.Serialize(detailsResponse);
        await context.Response.WriteAsync(result);
    }
}