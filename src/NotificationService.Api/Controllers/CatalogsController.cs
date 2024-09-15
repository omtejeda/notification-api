using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NotificationService.Api.Utils;
using System.Collections.Generic;
using NotificationService.Application.Contracts.RequestDtos;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Features.Catalogs.Commands.Delete;
using MediatR;
using NotificationService.Application.Features.Catalogs.Commands.Create;
using NotificationService.Application.Features.Catalogs.Queries.GetById;
using NotificationService.Application.Features.Catalogs.Queries.GetAll;

namespace NotificationService.Api.Controllers
{   
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class CatalogsController(ISender sender) : ApiController
    {
        private readonly ISender _sender = sender;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CatalogDto>>> GetAll([FromQuery] string name, bool? isActive, 
            string elementHasKey, string elementHasKeyValue, string elementHasLabelKey, string elementHasLabelKeyValue,
            int? page, int? pageSize)
        {
            var query = new GetAllCatalogsQuery
            {
                Name = name,
                IsActive = isActive,
                ElementHasKey = elementHasKey,
                ElementHasKeyValue = elementHasKeyValue,
                ElementHasLabelKey = elementHasLabelKey,
                ElementHasLabelKeyValue = elementHasLabelKeyValue,
                Page = page,
                PageSize = pageSize,
                Owner = CurrentPlatform.Name
            };

            var response = await _sender.Send(query);
            return Ok(response);
        }

        [HttpGet("{catalogId}")]
        public async Task<IActionResult> GetById([FromRoute] string catalogId, [FromQuery] string elementKey, string elementValue, string labelKey, string labelValue)
        {
            var query = new GetCatalogByIdQuery
            {
                CatalogId = catalogId,
                ElementKey = elementKey,
                ElementValue = elementValue,
                LabelKey = labelKey,
                LabelValue = labelValue,
                Owner = CurrentPlatform.Name
            };

            var response = await _sender.Send(query);
            return GetActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCatalogRequestDto request)
        {
            var command = new CreateCatalogCommand
            {
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive,
                Elements = request.Elements,
                Owner = CurrentPlatform.Name
            };

            var response = await _sender.Send(command);
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpDelete("{catalogId}")]
        public async Task<IActionResult> Delete([FromRoute] string catalogId)
        {
            var command = new DeleteCatalogCommand
            { 
                CatalogId = catalogId, 
                Owner = CurrentPlatform.Name
            };

            await _sender.Send(command);
            return Ok();
        }
    }
}