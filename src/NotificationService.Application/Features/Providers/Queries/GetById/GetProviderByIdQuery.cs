using NotificationService.Application.Common.Interfaces;
using NotificationService.Common.Dtos;

namespace NotificationService.Application.Features.Providers.Queries.GetById;

public record GetProviderByIdQuery : IQuery<BaseResponse<ProviderDto>>
{
    public GetProviderByIdQuery(string? providerId, string? owner)
    {
        ProviderId = providerId;
        Owner = owner;
    }
    
    public string? ProviderId { get; set; }
    public string? Owner { get; set; }
}