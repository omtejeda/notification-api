using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Utils;
using NotificationService.Application.Contracts.RequestDtos;
using NotificationService.Application.Features.Templates.Commands.Create;
using MediatR;
using NotificationService.Application.Features.Templates.Commands.Delete;
using NotificationService.Application.Features.Templates.Commands.UpdateContent;
using NotificationService.Application.Features.Templates.Queries.GetById;
using NotificationService.Application.Features.Templates.Queries.GetAll;
using NotificationService.Application.Features.Templates.Queries.GetContent;
using Swashbuckle.AspNetCore.Annotations;
using NotificationService.Api.Extensions;

namespace NotificationService.Api.Controllers.v1;

[ApiController]
[ApiVersion(ApiVersions.v1)]
[Route(Routes.ControllerRoute)]
public class TemplatesController(ISender sender) : ApiController
{
    private readonly ISender _sender = sender;

    [SwaggerOperation("Retrieves a list of all notification templates")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string name, string subject, string platformName, int? page, int? pageSize)
    {
        var query = new GetAllTemplatesQuery
        {
            Name = name,
            Subject = subject,
            PlatformName = platformName,
            Page = page,
            PageSize = pageSize,
            Owner = CurrentPlatform.Name
        };

        var response = await _sender.Send(query);
        return Ok(response);
    }

    [SwaggerOperation("Fetches the details of a specific notification template by its ID")]
    [HttpGet("{templateId}")]
    public async Task<IActionResult> GetById([FromRoute] string templateId)
    {
        var query = new GetTemplateByIdQuery(templateId, CurrentPlatform.Name);
        var response = await _sender.Send(query);
        return response.ToActionResult();
    }

    [SwaggerOperation("Previews the content of a specific notification template")]
    [HttpGet("{templateId}/preview")]
    public async Task<ContentResult> GetContent(string templateId)
    {
        var query = new GetTemplateContentQuery(templateId, CurrentPlatform.Name);
        var templateContent = await _sender.Send(query);

        bool found = templateContent is not null;
        
        return new ContentResult
        {
            ContentType = "text/html",
            StatusCode = found ? StatusCodes.Status200OK : StatusCodes.Status404NotFound,
            Content = templateContent?.Data?.Content
        };
    }

    [SwaggerOperation("Creates a new notification template with the provided content and metadata")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTemplateRequestDto request)
    {
        var command = new CreateTemplateCommand(request, CurrentPlatform.Name);
        var response = await _sender.Send(command);
        
        return StatusCode(StatusCodes.Status201Created, response);
    }

    [SwaggerOperation("Deletes a specific notification template by its ID")]
    [HttpDelete("{templateId}")]
    public async Task<IActionResult> Delete([FromRoute] string templateId)
    {
        var command = new DeleteTemplateCommand(templateId, CurrentPlatform.Name);
        
        await _sender.Send(command);
        return Ok();
    }

    [SwaggerOperation("Updates the content of a specific notification template")]
    [HttpPatch("{templateId}/content")]
    public async Task<IActionResult> UpdateContent([FromRoute] string templateId, [FromBody] UpdateTemplateContentRequestDto request)
    {
        var command = new UpdateTemplateContentCommand(templateId, request.Base64Content, CurrentPlatform.Name );
        await _sender.Send(command);

        return StatusCode(StatusCodes.Status204NoContent);
    }
}