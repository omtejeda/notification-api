using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NotificationService.Common.Enums;
using NotificationService.Core.Common.Exceptions;
using NotificationService.Common.Dtos;

namespace NotificationService.Api.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (RuleValidationException ex)
            {
                await Response(context, StatusCodes.Status400BadRequest, (int) ErrorCode.ValidationError, ex.Message);
            }
            catch (TimeoutException ex)
            {
                await Response(context, StatusCodes.Status408RequestTimeout, (int) ErrorCode.Warning, ex.Message);
            }
            catch (Exception ex)
            {
                await Response(context, StatusCodes.Status500InternalServerError, (int) ErrorCode.Error, ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private async Task Response(HttpContext context, int statusCode, int code, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var finalResponse = new FinalResponseDTO<object>(code, message);

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