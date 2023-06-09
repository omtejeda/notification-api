using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NotificationService.Utils;
using System.Collections.Generic;
using NotificationService.Dtos;
using NotificationService.Dtos.Requests;
using NotificationService.Services.Interfaces;
using LinqKit;
using NotificationService.Entities;

namespace NotificationService.Controllers
{   
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class CatalogsController : ApiController
    {
        private readonly ICatalogService _catalogService;

        public CatalogsController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CatalogDTO>>> Get([FromQuery] string name, bool? isActive, 
            string elementHasKey, string elementHasKeyValue, string elementHasLabelKey, string elementHasLabelKeyValue,
            int? page, int? pageSize)
        {
            var predicate = PredicateBuilder.New<Catalog>(true);
            var delimiter = ":";

            if (elementHasKeyValue?.Contains(delimiter) ?? false)
            {
                var keyValue = elementHasKeyValue.Split(delimiter);
                var key = keyValue.FirstOrDefault();
                var value = keyValue.LastOrDefault();

                predicate.And(x => x.Elements.Any(y => y.Key == key && y.Value == value));
            }

            if (elementHasLabelKeyValue?.Contains(delimiter) ?? false)
            {
                var keyValue = elementHasLabelKeyValue.Split(delimiter);
                var key = keyValue.FirstOrDefault();
                var value = keyValue.LastOrDefault();

                predicate.And(x => x.Elements.Any(y => y.Labels.Any(z => z.Key == key && z.Value == value)));
            }

            if (!string.IsNullOrWhiteSpace(name))
                predicate = predicate.And(x => x.Name == name);

            if (isActive.HasValue)
                predicate = predicate.And(x => x.IsActive == isActive);
            
            if (!string.IsNullOrWhiteSpace(elementHasKey))
                predicate = predicate.And(x => x.Elements.Any(y => y.Key == elementHasKey));
            
            if (!string.IsNullOrWhiteSpace(elementHasLabelKey))
                predicate = predicate.And(x => x.Elements.Any(y => y.Labels.Any(z => z.Key == elementHasLabelKey)));

            var response = await _catalogService.GetCatalogs(predicate, owner: Owner, page, pageSize);
            return Ok(response);
        }

        [HttpGet("{catalogId}")]
        public async Task<ActionResult<CatalogDTO>> GetById([FromRoute] string catalogId, [FromQuery] string elementKey, string elementValue, string labelKey, string labelValue)
        {
            var response = await _catalogService.GetCatalogById(catalogId, owner: Owner);
            if (response?.Data == null) return NotFound();
            
            if (!string.IsNullOrWhiteSpace(elementKey))
                response.Data.Elements = response.Data.Elements.Where(x => x.Key == elementKey).ToList();
            
            if (!string.IsNullOrWhiteSpace(elementKey) && !string.IsNullOrWhiteSpace(elementValue))
                response.Data.Elements = response.Data.Elements.Where(x => x.Key == elementKey && x.Value == elementValue).ToList();

            if (!string.IsNullOrWhiteSpace(labelKey))
                response.Data.Elements = response.Data.Elements.Where(x => x.Labels.Any(z => z.Key == labelKey)).ToList();
            
            if (!string.IsNullOrWhiteSpace(labelKey) && !string.IsNullOrWhiteSpace(labelValue))
                response.Data.Elements = response.Data.Elements.Where(x => x.Labels.Any(z => z.Key == labelKey && z.Value == labelValue)).ToList();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCatalogRequestDto request)
        {
            var platformCreated = await _catalogService.CreateCatalog(request.Name, request.Description, request.IsActive, request.Elements, owner: Owner);
            return StatusCode(StatusCodes.Status201Created, platformCreated);
        }

        [HttpDelete("{catalogId}")]
        public async Task<IActionResult> Delete([FromRoute] string catalogId)
        {
            await _catalogService.DeleteCatalog(catalogId, owner: Owner);
            return Ok();
        }
    }
}