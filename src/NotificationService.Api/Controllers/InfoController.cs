using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Utils;
using NotificationService.Api.Attributes;
using NotificationService.Common.Dtos;
using NotificationService.Common.Interfaces;

namespace NotificationService.Api.Controllers
{
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    [AllowAnonymous]
    public class InfoController : ApiController
    {
        private readonly IEnvironmentService _environmentService;
        private readonly IDateTimeService _dateTimeService;

        public InfoController(IEnvironmentService environmentService, IDateTimeService dateTimeService)
        {
            _environmentService = environmentService;
            _dateTimeService = dateTimeService;
        }

        [HttpGet]
        public IActionResult GetInfo()
        {
            var infoDto = new InfoDto()
            {
                IsProduction = _environmentService.IsProduction,
                Environment = _environmentService.CurrentEnvironment,
                SystemDate = _dateTimeService.UtcToLocalTime,
                Gmt = _environmentService.GmtOffset,
                LimitPageSize = _environmentService.LimitPageSize
            };

            var response = BaseResponse<InfoDto>.Success(infoDto);
            return Ok(response);
        }
    }
}