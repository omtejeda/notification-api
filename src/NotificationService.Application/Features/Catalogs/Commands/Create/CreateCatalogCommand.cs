using NotificationService.Common.Dtos;
using NotificationService.Common.Interfaces;
using NotificationService.Application.Contracts.ResponseDtos;

namespace NotificationService.Application.Features.Catalogs.Commands.Create;

public class CreateCatalogCommand : ICommand<BaseResponse<CatalogDto>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<ElementDto> Elements { get; set; } = [];
    public string Owner { get; set; } = string.Empty;
}