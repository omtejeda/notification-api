using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using NotificationService.Entities;
using NotificationService.Exceptions;
using NotificationService.Enums;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using NotificationService.Utils;
using NotificationService.Services.Interfaces;

namespace NotificationService.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        public HttpClientService(ILogger<HttpClientService> logger)
        {
            _httpClient = new HttpClient();
            _logger = logger;
        }

        public async Task<Tuple<bool, int, string>> SendHttpClient(string host, string uri, string verb, ICollection<HttpClientParam> parameters, string templateContent, ICollection<NotificationService.Dtos.Requests.MetadataDto> requestMetadata, string requestToDestination = null)
        {
            HttpUtil.CheckHTTPClientSettings(host, uri, verb);
            
            var queryString = new Dictionary<string, string>();
            foreach (var param in parameters)
            {
                _logger.LogInformation("Adding to {paramType}: {paramName}, reading value from: {readValueFrom}. Static value?: {isStaticValue}", param.Type.ToString(), param.Name, param.ReadValueFrom, param.HasStaticValue ?? false);
                    
                if (param.HasStaticValue == true)
                {
                    if (param.Type == HttpClientParamType.QueryString)
                        queryString.Add(param.Name, param.StaticValue);
                    
                    if (param.Type == HttpClientParamType.Header)
                        _httpClient.DefaultRequestHeaders.Add(param.Name, param.StaticValue);
                    
                    if (param.Type == HttpClientParamType.Route)
                        uri = uri.Replace($"{{{param.Name}}}", param.StaticValue);

                }
                
                if (param.ReadValueFrom == HttpClientParamValueReader.TemplateContent)
                {
                    if (param.Type == HttpClientParamType.QueryString)
                        queryString.Add(param.Name, templateContent);
                    
                    if (param.Type == HttpClientParamType.Header)
                        _httpClient.DefaultRequestHeaders.Add(param.Name, templateContent);

                    if (param.Type == HttpClientParamType.Route)
                        uri = uri.Replace($"{{{param.Name}}}", templateContent);
                }

                if (param.ReadValueFrom == HttpClientParamValueReader.RequestToDestination)
                {
                    if (param.Type == HttpClientParamType.QueryString)
                        queryString.Add(param.Name, requestToDestination);
                    
                    if (param.Type == HttpClientParamType.Header)
                        _httpClient.DefaultRequestHeaders.Add(param.Name, requestToDestination);

                    if (param.Type == HttpClientParamType.Route)
                        uri = uri.Replace($"{{{param.Name}}}", requestToDestination);
                }

                if (param.ReadValueFrom == HttpClientParamValueReader.RequestMetadata)
                {
                    var value = requestMetadata.Where(x => x.Key == param.Name).Select(x => x.Value).FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(value) && param.IsRequired == true) throw new RuleValidationException($"Provider require this metadata [{param.Name}] to be specified.");
                    
                    if (param.Type == HttpClientParamType.QueryString)
                        queryString.Add(param.Name, value);
                    
                    if (param.Type == HttpClientParamType.Header)
                        _httpClient.DefaultRequestHeaders.Add(param.Name, value);

                    if (param.Type == HttpClientParamType.Route)
                        uri = uri.Replace($"{{{param.Name}}}", value);
                }
            }

            var fullPath = HttpUtil.GetFullPath(host, uri, queryString);
            _logger.LogInformation("Path to send request to: {fullPath}", fullPath);
            
            var request = new HttpRequestMessage(new HttpMethod(verb), fullPath);
            try
            {
                var response = await _httpClient.SendAsync(request);
                var responseJson = await response.Content.ReadAsStringAsync();
                
                var code = (int) ErrorCode.HttpRequestOK;
                var message = $"Sent succesfully using HTTPClient. Status code: {response.StatusCode} with message: {responseJson}";
                var success = response.IsSuccessStatusCode;

                if (success)
                    _logger.LogInformation("HTTP request sent succesfully {statusCode} {message}", response.StatusCode, responseJson);
                else
                {
                    code = (int) ErrorCode.HttpRequestNotSent;
                    message = $"HTTP Request failed. Status code: {response.StatusCode} with message: {responseJson}";
                    _logger.LogWarning("HTTP request sent but failed. {statusCode} {message}", response.StatusCode, responseJson);
                }
                
                return (success, code, message).ToTuple();
            }
            catch (Exception e)
            {
                var errorMsg = $"An error ocurred trying to send HTTP request {e.Message}"; 
                _logger.LogError("An error ocurred trying to send HTTP request {message}", e.Message);
                return (success: false, code: (int) ErrorCode.HttpRequestNotSent, message: errorMsg).ToTuple();
            }
        }
    }
}