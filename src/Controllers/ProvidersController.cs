using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NotificationService.Entities;
using NotificationService.Utils;
using NotificationService.Enums;
using NotificationService.Repositories;
using NotificationService.Dtos;
using NotificationService.Dtos.Requests;
using NotificationService.Services.Interfaces;

namespace NotificationService.Controllers
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
            var providerTypes = NotificationService.Helpers.EnumHelper.GetProviderTypes();
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
    }
}