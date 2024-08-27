using Microsoft.AspNetCore.Mvc;
using NotificationService.Common.Dtos;
namespace NotificationService.Api.Controllers
{   
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected PlatformDto GetCurrentPlatform()
        {
            var currentPlatform = HttpContext.Items[nameof(PlatformDto)] as PlatformDto;
            return currentPlatform;
        }

        protected string Owner => GetCurrentPlatform()?.Name;
    }
}