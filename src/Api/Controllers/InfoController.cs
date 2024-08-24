using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Utils;
using NotificationService.Core.Common.Dtos;
using NotificationService.Core.Common.Enums;
using NotificationService.Api.Attributes;
using NotificationService.Contracts.ResponseDtos;

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
            var response = new FinalResponseDTO<InfoDTO>((int) ErrorCode.OK, new InfoDTO());
            return Ok(response);
        }
    }
}