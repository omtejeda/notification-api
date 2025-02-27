using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Utils;
using NotificationService.Application.Contracts.DTOs.Requests;
using NotificationService.Application.Contracts.DTOs.Responses;
using NotificationService.Application.Features.Catalogs.Commands.Delete;
using MediatR;
using NotificationService.Application.Features.Catalogs.Commands.Create;
using NotificationService.Application.Features.Catalogs.Queries.GetById;
using NotificationService.Application.Features.Catalogs.Queries.GetAll;
using Swashbuckle.AspNetCore.Annotations;
using NotificationService.Api.Extensions;

namespace NotificationService.Api.Controllers.v1;

[ApiController]
[ApiVersion(ApiVersions.v1)]
[Route(Routes.ControllerRoute)]
public class CatalogsController(ISender sender) : ApiController
{
    private readonly ISender _sender = sender;

    [SwaggerOperation("Retrieves a list of all available catalogs")]
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

    [SwaggerOperation("Fetches details of a specific catalog by its ID")]
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
        return response.ToActionResult();
    }

    [SwaggerOperation("Creates a new catalog with the provided information")]
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

    [SwaggerOperation("Deletes a catalog identified by its ID")]
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