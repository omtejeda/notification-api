using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NotificationService.Utils;
using NotificationService.Enums;
using NotificationService.Dtos;
using NotificationService.Dtos.Requests;
using NotificationService.Services.Interfaces;
using NotificationService.Attributes;

namespace NotificationService.Controllers
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
            var response = await _platformService.GetPlatforms(x => (x.Name == name || name == null) && (x.IsActive == isActive || isActive == null), owner: Owner, page, pageSize);
            return Ok(response);
        }

        [HttpGet("{platformId}")]
        public async Task<IActionResult> GetById([FromRoute] string platformId)
        {
            var response = await _platformService.GetPlatformById(platformId, owner: Owner);
            if (response?.Data == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("me")]
        public IActionResult GetMe()
        {
            var platformDTO = GetCurrentPlatform();
            var response = new FinalResponseDTO<PlatformDTO>((int) ErrorCode.OK, platformDTO);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreatePlatformRequestDto request)
        {
            var platformCreated = await _platformService.CreatePlatform(request.Name, request.Description, owner: Owner);
            return StatusCode(StatusCodes.Status201Created, platformCreated);
        }

        [HttpDelete("{platformId}")]
        public async Task<IActionResult> Delete([FromRoute] string platformId)
        {
            await _platformService.DeletePlatform(platformId, owner: Owner);
            return Ok();
        }
    }
}