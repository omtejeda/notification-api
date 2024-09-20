using NotificationService.Domain.Entities;
using NotificationService.Domain.Dtos;
using NotificationService.Domain.Models;

namespace NotificationService.Application.Features.Providers.Interfaces;

public interface IHttpClientProvider
{
    Task<NotificationResult> SendHttpClient(HttpClientSetting httpClientSetting, string templateContent, ICollection<MetadataDto> requestMetadata, string requestToDestination);
}