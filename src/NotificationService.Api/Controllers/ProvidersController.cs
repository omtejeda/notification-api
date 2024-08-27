using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NotificationService.Common.Entities;
using NotificationService.Api.Utils;
using NotificationService.Common.Enums;
using NotificationService.Contracts.RequestDtos;
using NotificationService.Contracts.Interfaces.Services;
using NotificationService.Contracts.Interfaces.Repositories;
using NotificationService.Common.Dtos;

namespace NotificationService.Api.Controllers
{   
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class ProvidersController : ApiController
    {
        private readonly IRepository<Provider> _providerRepository;
        private readonly IProviderService _providerService;

        public ProvidersController(IRepository<Provider> providerRepository, IProviderService providerService)
        {
            _providerRepository = providerRepository;
            _providerService = providerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string name, string type, int? page, int? pageSize)
        {
            Enum.TryParse(type, out ProviderType providerType);
            
            var response = await _providerService.GetProviders(x => (x.Name == name || name == null) && (x.Type == providerType || type == null), owner: Owner, page: page, pageSize: pageSize);
            return Ok(response);
        }

        [HttpGet("{providerId}")]
        public async Task<IActionResult> Get(string providerId)
        {
            var response = await _providerService.GetProviderById(providerId, owner: Owner);
            if (response?.Data == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("types")]
        public IActionResult GetProviderTypes()
        {
            var providerTypes = Core.Common.Helpers.EnumHelper.GetProviderTypes();
            var response = new FinalResponseDTO<object>((int) ErrorCode.OK, providerTypes);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProviderRequestDto request)
        {
            var providerCreated = await _providerService.CreateProvider(request, owner: Owner);
            return StatusCode(StatusCodes.Status201Created, providerCreated);
        }

        [HttpDelete("{providerId}")]
        public async Task<IActionResult> Delete([FromRoute] string providerId)
        {
            await _providerService.DeleteProvider(providerId, owner: Owner);
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpPost("{providerId}/whitelist")]
        public async Task<ActionResult> AddToWhiteList([FromRoute] string providerId, [FromBody] AddToWhiteListRequestDto request)
        {
            await _providerService.AddToWhiteList(providerId, owner: Owner, request.Recipient);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpDelete("{providerId}/whitelist")]
        public async Task<ActionResult> DeleteFromWhiteList([FromRoute] string providerId, [FromBody] DeleteFromWhiteListRequestDto request)
        {
            await _providerService.DeleteFromWhiteList(providerId, owner: Owner, request.Recipient);

            return StatusCode(StatusCodes.Status200OK);
        }
    }
}