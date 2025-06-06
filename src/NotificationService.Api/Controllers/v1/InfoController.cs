using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Utils;
using NotificationService.Api.Attributes;
using NotificationService.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Api.Controllers.v1;

[ApiController]
[ApiVersion(ApiVersions.v1)]
[Route(Routes.ControllerRoute)]
[AllowAnonymous]

public class InfoController(
    IEnvironmentService environmentService,
    IDateTimeService dateTimeService) 
    : ApiController
{
    private readonly IEnvironmentService _environmentService = environmentService;
    private readonly IDateTimeService _dateTimeService = dateTimeService;

    [SwaggerOperation("Retrieve general information about Notification API")]
    [HttpGet]
    public ActionResult<BaseResponse<InfoDto>> GetInfo()
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

    [SwaggerOperation("Fetch a list of status or error codes used by Notification API")]
    [HttpGet("codes")]
    public IActionResult Get([FromQuery] int? code)
    {
        var codes = Application.Common.Helpers.EnumHelper.GetCodesAndItsDescription(code);

        var response = BaseResponse<object>.Success(codes);
        return Ok(response);
    }
}