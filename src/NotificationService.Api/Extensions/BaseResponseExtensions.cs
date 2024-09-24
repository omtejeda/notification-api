using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Common.Models;

namespace NotificationService.Api.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="BaseResponse{T}"/> class.
/// This class includes methods that facilitate the conversion of a <see cref="BaseResponse{T}"/> 
/// into an <see cref="ActionResult"/> for use in controllers.
/// </summary>
public static class BaseResponseExtensions
{
    /// <summary>
    /// Converts a <see cref="BaseResponse{T}"/> into an <see cref="ActionResult"/> 
    /// suitable for controller's endpoints responses.
    /// </summary>
    /// <typeparam name="T">The type of data contained in the <see cref="BaseResponse{T}"/>.</typeparam>
    /// <param name="response">The <see cref="BaseResponse{T}"/> to convert.</param>
    /// <returns>An <see cref="ActionResult"/> that represents the result of the conversion. 
    /// Returns a <see cref="NotFoundObjectResult"/> if the <see cref="Data"/> is null; 
    /// otherwise, returns an <see cref="OkObjectResult"/> containing the response.</returns>
    public static ActionResult ToActionResult<T>(this BaseResponse<T> response) where T : class
    {
        return response?.Data switch
        {
            null => new NotFoundObjectResult(response),
            _ => new OkObjectResult(response)
        };
    }
}