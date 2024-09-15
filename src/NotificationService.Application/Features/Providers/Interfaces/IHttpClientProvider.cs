using NotificationService.Domain.Entities;
using NotificationService.Domain.Dtos;

namespace NotificationService.Application.Features.Providers.Interfaces;

public interface IHttpClientProvider
{
    Task<Tuple<bool, int, string>> SendHttpClient(HttpClientSetting httpClientSetting, string templateContent, ICollection<MetadataDto> requestMetadata, string requestToDestination = null);
}