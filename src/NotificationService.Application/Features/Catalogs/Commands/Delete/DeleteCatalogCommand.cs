using NotificationService.Common.Interfaces;

namespace NotificationService.Application.Features.Catalogs.Commands.Delete;

public record DeleteCatalogCommand : ICommand
{
    public string? CatalogId { get; init; }
    public string? Owner { get; set;}
}