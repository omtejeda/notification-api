using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Common.Models;
namespace NotificationService.Api.Controllers
{   
    [ApiController]
    public class ApiController() : ControllerBase
    {
        protected PlatformDto CurrentPlatform
        { 
            get
            {
                return HttpContext.Items[nameof(PlatformDto)] as PlatformDto;
            }
        }

        protected IActionResult GetActionResult<T>(BaseResponse<T> response) where T : class 
            => (response?.Data is null) ? NotFound() : Ok(response);
    }
}