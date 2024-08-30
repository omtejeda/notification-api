using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Utils;
using NotificationService.Common.Enums;
using NotificationService.Api.Attributes;
using NotificationService.Common.Dtos;

namespace NotificationService.Api.Controllers
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
            var response = BaseResponse<InfoDto>.Success(new InfoDto());
            return Ok(response);
        }
    }
}