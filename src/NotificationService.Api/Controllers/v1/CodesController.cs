using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Utils;
using NotificationService.Common.Dtos;

namespace NotificationService.Api.Controllers.v1
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
            var codes = Application.Common.Helpers.EnumHelper.GetCodesAndItsDescription(code);

            var response = BaseResponse<object>.Success(codes);
            return Ok(response);
        }
    }
}