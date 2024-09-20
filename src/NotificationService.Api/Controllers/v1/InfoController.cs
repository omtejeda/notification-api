using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Utils;
using NotificationService.Api.Attributes;
using NotificationService.Common.Dtos;
using NotificationService.Common.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace NotificationService.Api.Controllers.v1;

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

    [SwaggerOperation("Retrieves general information about Notification API")]
    [HttpGet]
    public IActionResult GetInfo()
    {
        var infoDto = new InfoDto()
        {
            IsProduction = _environmentService.IsProduction,
            Environment = _environmentService.CurrentEnvironment,
            SystemDate = _dateTimeService.UtcToLocalTime,
            Gmt = _environmentService.GmtOffset
        };

        var response = BaseResponse<InfoDto>.Success(infoDto);
        return Ok(response);
    }

    [SwaggerOperation("Fetches a list of status or error codes used by Notification API")]
    [HttpGet("codes")]
    public IActionResult Get([FromQuery] int? code)
    {
        var codes = Application.Common.Helpers.EnumHelper.GetCodesAndItsDescription(code);

        var response = BaseResponse<object>.Success(codes);
        return Ok(response);
    }
}