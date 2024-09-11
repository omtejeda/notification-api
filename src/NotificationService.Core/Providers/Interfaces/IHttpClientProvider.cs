using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using NotificationService.Domain.Entities;
using NotificationService.Contracts.RequestDtos;
using NotificationService.Domain.Dtos;

namespace NotificationService.Core.Providers.Interfaces
{
    public interface IHttpClientProvider
    {
        Task<Tuple<bool, int, string>> SendHttpClient(HttpClientSetting httpClientSetting, string templateContent, ICollection<MetadataDto> requestMetadata, string requestToDestination = null);
    }
}