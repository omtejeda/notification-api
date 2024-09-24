using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Common.Models;

namespace NotificationService.Api.Extensions;

public static class BaseResponseExtensions
{
    public static ActionResult ToActionResult<T>(this BaseResponse<T> response) where T : class
    {
        return response?.Data switch
        {
            null => new NotFoundObjectResult(response),
            _ => new OkObjectResult(response)
        };
    }
}