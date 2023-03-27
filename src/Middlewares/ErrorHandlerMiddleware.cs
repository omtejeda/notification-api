using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using NotificationService.Enums;
using NotificationService.Dtos;
using NotificationService.Exceptions;

namespace NotificationService.Middlewares
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
                    IgnoreNullValues = true
                });
            await context.Response.WriteAsync(result);
        }
    }
}