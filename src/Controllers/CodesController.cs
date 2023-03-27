using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Entities;
using NotificationService.Utils;
using NotificationService.Enums;
using NotificationService.Dtos;
namespace NotificationService.Controllers
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
            var codes = NotificationService.Helpers.EnumHelper.GetCodesAndItsDescription(code);

            var response = new FinalResponseDTO<object>((int) ErrorCode.OK, codes);
            return Ok(response);
        }
    }
}