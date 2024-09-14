using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NotificationService.Api.Utils;
using NotificationService.Application.Contracts.RequestDtos;
using NotificationService.Application.Features.Templates.Commands.Create;
using MediatR;
using NotificationService.Application.Features.Templates.Commands.Delete;
using NotificationService.Application.Features.Templates.Commands.UpdateContent;
using NotificationService.Application.Features.Templates.Queries.GetById;
using NotificationService.Application.Features.Templates.Queries.GetAll;
using NotificationService.Application.Features.Templates.Queries.GetContent;

namespace NotificationService.Api.Controllers
{   
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class TemplatesController : ApiController
    {
        private readonly ISender _sender;

        public TemplatesController(ISender sender)
        {
            _sender = sender;
        }

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

        [HttpGet("{templateId}")]
        public async Task<IActionResult> GetById([FromRoute] string templateId)
        {
            var query = new GetTemplateByIdQuery(templateId, CurrentPlatform.Name);
            var response = await _sender.Send(query);
            return GetActionResult(response);
        }


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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTemplateRequestDto request)
        {
            var command = new CreateTemplateCommand
            {
                RequestDto = request,
                Owner = CurrentPlatform.Name
            };

            var response = await _sender.Send(command);
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpDelete("{templateId}")]
        public async Task<IActionResult> Delete([FromRoute] string templateId)
        {
            var command = new DeleteTemplateCommand
            { 
                TemplateId = templateId, 
                Owner = CurrentPlatform.Name 
            };
            
            await _sender.Send(command);
            return Ok();
        }

        [HttpPatch("{templateId}/content")]
        public async Task<IActionResult> UpdateContent([FromRoute] string templateId, [FromBody] UpdateTemplateContentRequestDto request)
        {
            var command = new UpdateTemplateContentCommand
            {
                TemplateId = templateId,
                Base64Content = request.Base64Content,
                Owner = CurrentPlatform.Name
            };

            await _sender.Send(command);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}