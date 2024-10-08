using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Features.Providers.Queries.GetById;

public record GetProviderByIdQuery : IQuery<BaseResponse<ProviderDto>>
{
    public GetProviderByIdQuery(string providerId, string owner)
    {
        ProviderId = providerId;
        Owner = owner;
    }
    
    public string ProviderId { get; set; }
    public string Owner { get; set; }
}