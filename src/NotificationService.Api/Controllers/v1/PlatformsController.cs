using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Utils;
using NotificationService.Api.Attributes;
using NotificationService.Application.Contracts.DTOs.Requests;
using MediatR;
using NotificationService.Application.Features.Platforms.Commands.Create;
using NotificationService.Application.Features.Platforms.Commands.Delete;
using NotificationService.Application.Features.Platforms.Queries.GetById;
using NotificationService.Application.Features.Platforms.Queries.GetAll;
using Swashbuckle.AspNetCore.Annotations;
using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Common.Models;
using NotificationService.Api.Extensions;

namespace NotificationService.Api.Controllers.v1;

[ApiController]
[ApiVersion(ApiVersions.v1)]
[Route(Routes.ControllerRoute)]
public class PlatformsController(ISender sender) : ApiController
{
    private readonly ISender _sender = sender;

    [SwaggerOperation("Retrieves a list of all platforms registered")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string name, bool? isActive, int? page, int? pageSize)
    {
        var query = new GetAllPlatformsQuery
        {
            Name = name,
            IsActive = isActive,
            Page = page,
            PageSize = pageSize,
            Owner = CurrentPlatform.Name
        };

        var response = await _sender.Send(query);
        return Ok(response);
    }

    [SwaggerOperation("Fetches details of a specific platform by its ID")]
    [HttpGet("{platformId}")]
    public async Task<IActionResult> GetById([FromRoute] string platformId)
    {
        var query = new GetPlatformByIdQuery(platformId, CurrentPlatform.Name);
        var response = await _sender.Send(query);

        return response.ToActionResult();
    }

    [SwaggerOperation("Retrieves the platform information associated with the current apikey")]
    [HttpGet("me")]
    public IActionResult GetCurrent()
    {
        PlatformDto platformDto = CurrentPlatform;
        var response = BaseResponse<PlatformDto>.Success(platformDto);
        
        return Ok(response);
    }

    [SwaggerOperation("Register a new platform for sending notifications")]
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePlatformRequestDto request)
    {
        var command = new CreatePlatformCommand
        {
            Name = request.Name,
            Description = request.Description,
            Owner = CurrentPlatform?.Name ?? request.Name
        };

        var response = await _sender.Send(command);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    [SwaggerOperation("Deletes a platform identified by its ID")]
    [HttpDelete("{platformId}")]
    public async Task<IActionResult> Delete([FromRoute] string platformId)
    {
        var command = new DeletePlatformCommand
        {
            PlatformId = platformId,
            Owner = CurrentPlatform.Name
        };

        await _sender.Send(command);
        return Ok();
    }
}