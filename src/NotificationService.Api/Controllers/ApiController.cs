using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Common.Dtos;

namespace NotificationService.Api.Controllers;

/// <summary>
/// Serves as a base class for API controllers, 
/// providing common functionality and shared properties for derived controllers.
/// </summary>
[ApiController]
public class ApiController : ControllerBase
{
    /// <summary>
    /// Gets the current platform from the HTTP context.
    /// </summary>
    protected PlatformDto CurrentPlatform
    { 
        get
        {
            return HttpContext.Items[nameof(PlatformDto)] as PlatformDto;
        }
    }
}