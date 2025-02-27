using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.DTOs.Responses;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Features.Catalogs.Commands.Create;

public class CreateCatalogCommand : ICommand<BaseResponse<CatalogDto>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<ElementDto> Elements { get; set; } = [];
    public string Owner { get; set; } = string.Empty;
}