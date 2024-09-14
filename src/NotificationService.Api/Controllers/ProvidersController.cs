using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NotificationService.Api.Utils;
using NotificationService.Application.Contracts.RequestDtos;
using NotificationService.Common.Dtos;
using NotificationService.Application.Features.Providers.Commands.Create;
using NotificationService.Application.Features.Providers.Commands.Delete;
using NotificationService.Application.Features.Providers.Commands.AddToWhiteList;
using NotificationService.Application.Features.Providers.Commands.RemoveFromWhiteList;
using NotificationService.Application.Features.Providers.Queries.GetById;
using NotificationService.Application.Features.Providers.Queries.GetAll;
using MediatR;

namespace NotificationService.Api.Controllers
{   
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class ProvidersController : ApiController
    {
        private readonly ISender _sender;

        public ProvidersController(ISender sender)
        {
            _sender = sender;
        }

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

        [HttpGet("{providerId}")]
        public async Task<IActionResult> GetById(string providerId)
        {
            var query = new GetProviderByIdQuery(providerId, CurrentPlatform.Name);
            var response = await _sender.Send(query);
            
            return GetActionResult(response);
        }

        [HttpGet("types")]
        public IActionResult GetProviderTypes()
        {
            var providerTypes = Application.Common.Helpers.EnumHelper.GetProviderTypes();
            var response = BaseResponse<object>.Success(providerTypes);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProviderRequestDto request)
        {
            var command = new CreateProviderCommand(request, CurrentPlatform.Name);
            var response = await _sender.Send(command);
            
            return StatusCode(StatusCodes.Status201Created, response);
        }

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

        [HttpPost("{providerId}/whitelist")]
        public async Task<ActionResult> AddToWhiteList([FromRoute] string providerId, [FromBody] AddToWhiteListRequestDto request)
        {
            var command = new AddToWhiteListCommand(providerId, request.Recipient, CurrentPlatform.Name);
            await _sender.Send(command);
            
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpDelete("{providerId}/whitelist")]
        public async Task<ActionResult> DeleteFromWhiteList([FromRoute] string providerId, [FromBody] DeleteFromWhiteListRequestDto request)
        {
            var command = new RemoveFromWhiteListCommand(providerId, request.Recipient, CurrentPlatform.Name);
            await _sender.Send(command);

            return StatusCode(StatusCodes.Status200OK);
        }
    }
}