using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using NotificationService.Entities;

namespace NotificationService.Services.Interfaces
{
    public interface IHttpClientService
    {
        Task<Tuple<bool, int, string>> SendHttpClient(string host, string uri, string verb, ICollection<HttpClientParam> parameters, string templateContent, ICollection<NotificationService.Dtos.Requests.MetadataDto> requestMetadata, string requestToDestination = null);
    }
}