using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NotificationService.Api.Utils;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Api.Attributes;
using NotificationService.Application.Contracts.RequestDtos;
using NotificationService.Common.Dtos;
using MediatR;
using NotificationService.Application.Features.Platforms.Commands.CreatePlatform;

namespace NotificationService.Api.Controllers
{   
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class PlatformsController : ApiController
    {
        private readonly IPlatformService _platformService;
        private readonly IMediator _mediator;

        public PlatformsController(IPlatformService platformService, IMediator mediator)
        {
            _platformService = platformService;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string name, bool? isActive, int? page, int? pageSize)
        {
            var response = await _platformService
                .GetPlatforms(x => (x.Name == name || name == null) && 
                    (x.IsActive == isActive || isActive == null),
                    owner: CurrentPlatform.Name,
                    page,
                    pageSize);
            return Ok(response);
        }

        [HttpGet("{platformId}")]
        public async Task<IActionResult> GetById([FromRoute] string platformId)
        {
            var response = await _platformService.GetPlatformById(platformId, owner: CurrentPlatform.Name);
            if (response?.Data == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("me")]
        public IActionResult GetMe()
        {
            PlatformDto platformDto = CurrentPlatform;
            var response = BaseResponse<PlatformDto>.Success(platformDto);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreatePlatformRequestDto request)
        {
            var command = new CreatePlatformCommand
            {
                Name = request.Name,
                Description = request.Description,
                Owner = request.Name
            };

            var result = await _mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpDelete("{platformId}")]
        public async Task<IActionResult> Delete([FromRoute] string platformId)
        {
            await _platformService.DeletePlatform(platformId, owner: CurrentPlatform.Name);
            return Ok();
        }
    }
}