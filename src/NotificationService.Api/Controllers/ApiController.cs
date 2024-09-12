using Microsoft.AspNetCore.Mvc;
using NotificationService.Common.Dtos;
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
    }
}