using Microsoft.AspNetCore.Mvc;
using NotificationService.Dtos;
namespace NotificationService.Controllers
{   
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected PlatformDTO GetCurrentPlatform()
        {
            var currentPlatform = HttpContext.Items[nameof(PlatformDTO)] as PlatformDTO;
            return currentPlatform;
        }

        protected string Owner => GetCurrentPlatform()?.Name;
    }
}