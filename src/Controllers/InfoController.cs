using Microsoft.AspNetCore.Mvc;
using NotificationService.Utils;
using NotificationService.Dtos;
using NotificationService.Enums;
using NotificationService.Attributes;

namespace NotificationService.Controllers
{
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    [AllowAnonymous]
    public class InfoController : ApiController
    {
        [HttpGet]
        public IActionResult GetInfo()
        {
            var response = new FinalResponseDTO<InfoDTO>((int) ErrorCode.OK, new InfoDTO());
            return Ok(response);
        }
    }
}