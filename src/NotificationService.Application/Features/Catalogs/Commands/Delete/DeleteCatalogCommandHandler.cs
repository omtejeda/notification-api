using NotificationService.Common.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;

namespace NotificationService.Application.Features.Catalogs.Commands.Delete;

public class DeleteCatalogCommandHandler(ICatalogService catalogService) : ICommandHandler<DeleteCatalogCommand>
{
    private readonly ICatalogService _catalogService = catalogService;

    public async Task Handle(DeleteCatalogCommand request, CancellationToken cancellationToken)
    {
        await _catalogService.DeleteCatalog(request.CatalogId!, request.Owner!);
    }
}