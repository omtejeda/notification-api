using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NotificationService.Api.Utils;
using NotificationService.Domain.Enums;
using NotificationService.Contracts.Interfaces.Services;
using NotificationService.Api.Attributes;
using NotificationService.Contracts.RequestDtos;
using NotificationService.Common.Dtos;

namespace NotificationService.Api.Controllers
{   
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class PlatformsController : ApiController
    {
        private readonly IPlatformService _platformService;

        public PlatformsController(IPlatformService platformService)
        {
            _platformService = platformService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string name, bool? isActive, int? page, int? pageSize)
        {
            var response = await _platformService.GetPlatforms(x => (x.Name == name || name == null) && (x.IsActive == isActive || isActive == null), owner: CurrentPlatform.Name, page, pageSize);
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
            var platformCreated = await _platformService.CreatePlatform(request.Name, request.Description, owner: CurrentPlatform.Name);
            return StatusCode(StatusCodes.Status201Created, platformCreated);
        }

        [HttpDelete("{platformId}")]
        public async Task<IActionResult> Delete([FromRoute] string platformId)
        {
            await _platformService.DeletePlatform(platformId, owner: CurrentPlatform.Name);
            return Ok();
        }
    }
}