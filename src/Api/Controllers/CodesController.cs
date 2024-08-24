using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Utils;
using NotificationService.Core.Common.Enums;
using NotificationService.Contracts.ResponseDtos;
namespace NotificationService.Api.Controllers
{   
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class CodesController : ApiController
    {
        public CodesController() {}

        [HttpGet]
        public IActionResult Get([FromQuery] int? code)
        {
            var codes = Core.Common.Helpers.EnumHelper.GetCodesAndItsDescription(code);

            var response = new FinalResponseDTO<object>((int) ErrorCode.OK, codes);
            return Ok(response);
        }
    }
}