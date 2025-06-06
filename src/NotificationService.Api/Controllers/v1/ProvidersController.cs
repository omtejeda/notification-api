using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Utils;
using NotificationService.Application.Contracts.DTOs.Requests;
using NotificationService.Application.Features.Providers.Commands.Create;
using NotificationService.Application.Features.Providers.Commands.Delete;
using NotificationService.Application.Features.Providers.Commands.AddToWhiteList;
using NotificationService.Application.Features.Providers.Commands.RemoveFromWhiteList;
using NotificationService.Application.Features.Providers.Queries.GetById;
using NotificationService.Application.Features.Providers.Queries.GetAll;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using NotificationService.Application.Common.Models;
using NotificationService.Api.Extensions;

namespace NotificationService.Api.Controllers.v1;

[ApiController]
[ApiVersion(ApiVersions.v1)]
[Route(Routes.ControllerRoute)]
public class ProvidersController(ISender sender) : ApiController
{
    private readonly ISender _sender = sender;

    [SwaggerOperation("Retrieve a list of all available providers for notification delivery")]
    [HttpGet]
    public async Task<IActionResult> GetAll(string name, string type, int? page, int? pageSize)
    {
        var query = new GetAllProvidersQuery
        {
            Name = name,
            Type = type,
            Page = page,
            PageSize = pageSize,
            Owner = CurrentPlatform.Name
        };

        var response = await _sender.Send(query);
        return Ok(response);
    }

    [SwaggerOperation("Fetch the details of a specific provider by its identifier")]
    [HttpGet("{providerId}")]
    public async Task<IActionResult> GetById(string providerId)
    {
        var query = new GetProviderByIdQuery(providerId, CurrentPlatform.Name);
        var response = await _sender.Send(query);
        
        return response.ToActionResult();
    }

    [SwaggerOperation("Retrieve a list of all provider types (e.g., SMTP, SendGrid, HttpClient)")]
    [HttpGet("types")]
    public IActionResult GetProviderTypes()
    {
        var providerTypes = Application.Common.Helpers.EnumHelper.GetProviderTypes();
        var response = BaseResponse<object>.Success(providerTypes);
        return Ok(response);
    }

    [SwaggerOperation("Register a new provider for notification delivery")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProviderRequestDto request)
    {
        var command = new CreateProviderCommand(request, CurrentPlatform.Name);
        var response = await _sender.Send(command);
        
        return StatusCode(StatusCodes.Status201Created, response);
    }

    [SwaggerOperation("Delete a notification provider identified by its identifier")]
    [HttpDelete("{providerId}")]
    public async Task<IActionResult> Delete([FromRoute] string providerId)
    {
        var command = new DeleteProviderCommand
        {
            ProviderId = providerId,
            Owner = CurrentPlatform.Name
        };

        await _sender.Send(command);
        return Ok();
    }

    [SwaggerOperation("Add a recipient to the whitelist for sending notifications with a specific provider")]
    [HttpPost("{providerId}/whitelist")]
    public async Task<ActionResult> AddToWhiteList([FromRoute] string providerId, [FromBody] AddToWhiteListRequestDto request)
    {
        var command = new AddToWhiteListCommand(providerId, request.Recipient, CurrentPlatform.Name);
        await _sender.Send(command);
        
        return StatusCode(StatusCodes.Status201Created);
    }

    [SwaggerOperation("Remove a recipient from the whitelist")]
    [HttpDelete("{providerId}/whitelist")]
    public async Task<ActionResult> DeleteFromWhiteList([FromRoute] string providerId, [FromBody] DeleteFromWhiteListRequestDto request)
    {
        var command = new RemoveFromWhiteListCommand(providerId, request.Recipient, CurrentPlatform.Name);
        await _sender.Send(command);

        return StatusCode(StatusCodes.Status200OK);
    }
}