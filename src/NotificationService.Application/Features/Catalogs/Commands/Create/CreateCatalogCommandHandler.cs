using NotificationService.Common.Interfaces;
using NotificationService.Common.Dtos;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Contracts.ResponseDtos;

namespace NotificationService.Application.Features.Catalogs.Commands.Create;

public class CreateCatalogCommandHandler(ICatalogService catalogService) 
    : ICommandHandler<CreateCatalogCommand, BaseResponse<CatalogDto>>
{
    private readonly ICatalogService _catalogService = catalogService;

    public async Task<BaseResponse<CatalogDto>> Handle(CreateCatalogCommand request, CancellationToken cancellationToken)
    {
        var result = await _catalogService.CreateCatalog(
                request.Name!,
                request.Description!,
                request.IsActive, 
                request.Elements, 
                owner: request.Owner!);
        return result;
    }
}