using NotificationService.Domain.Entities;
using NotificationService.Application.Exceptions;
using NotificationService.Domain.Enums;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Utils;
using System.Text;
using AutoMapper;
using NotificationService.Domain.Dtos;
using NotificationService.Common.Resources;
using NotificationService.Application.Features.Providers.Interfaces;

namespace NotificationService.Infrastructure.Providers;

public class HttpClientProvider : IHttpClientProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    public HttpClientProvider(ILogger<HttpClientProvider> logger, IHttpClientFactory httpClientFactory, IMapper mapper)
    {
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Tuple<bool, int, string>> SendHttpClient(HttpClientSetting httpClientSetting, string templateContent, ICollection<MetadataDto> requestMetadata, string requestToDestination = null)
    {
        HttpUtil.CheckHTTPClientSettings(httpClientSetting.Host, httpClientSetting.Uri, httpClientSetting.Verb);
        
        var queryString = new Dictionary<string, string>();

        foreach (var param in httpClientSetting.Params)
        {
            _logger.LogInformation("Adding to {paramType}: {paramName}, reading value from: {readValueFrom}. Static value?: {isStaticValue}", param.Type.ToString(), param.Name, param.ReadValueFrom, param.HasStaticValue ?? false);

            var value = GetParamValue(httpClientParam: param,
                requestMetadata: requestMetadata,
                templateContent: templateContent,
                requestToDestination: requestToDestination);
            
            if (!param.IsValid(value))
                throw new RuleValidationException(string.Format(Messages.MetadataRequiredByProvider, param.Name));
            
            if (param.Type == HttpClientParamType.QueryString)
                SetQueryString(queryString, param.Name, value);
            
            if (param.Type == HttpClientParamType.Header)
                SetHeader(param.Name, value);

            if (param.Type == HttpClientParamType.Route)
                SetRoute(uri: httpClientSetting.Uri, param.Name, value);
            
            if (param.Type == HttpClientParamType.JsonBodyDefinition)
                SetMetadataToJsonBodyDefinition(metadata: requestMetadata, param.Name, value);
        }

        var fullPath = HttpUtil.GetFullPath(httpClientSetting.Host, httpClientSetting.Uri, queryString);
        _logger.LogInformation("Path to send request to: {fullPath}", fullPath);            

        var jsonBody = GetJson(httpClientSetting.JsonBody, requestMetadata);
        var request = NewHttpRequest(httpClientSetting.Verb, fullPath, jsonBody);

        try
        {
            var response = await _httpClient.SendAsync(request);
            var responseJson = await response.Content.ReadAsStringAsync();
            
            var code = (int) ResultCode.HttpRequestOK;
            var message = $"Sent succesfully using HTTPClient. Status code: {response.StatusCode} with message: {responseJson}";
            var success = response.IsSuccessStatusCode;

            if (success)
                _logger.LogInformation("HTTP request sent succesfully {statusCode} {message}", response.StatusCode, responseJson);
            else
            {
                code = (int) ResultCode.HttpRequestNotSent;
                message = $"HTTP Request failed. Status code: {response.StatusCode} with message: {responseJson}";
                _logger.LogWarning("HTTP request sent but failed. {statusCode} {message}", response.StatusCode, responseJson);
            }
            
            return (success, code, message).ToTuple();
        }
        catch (Exception e)
        {
            var errorMsg = $"An error ocurred trying to send HTTP request {e.Message}"; 
            _logger.LogError("An error ocurred trying to send HTTP request {message}", e.Message);
            return (success: false, code: (int) ResultCode.HttpRequestNotSent, message: errorMsg).ToTuple();
        }
    }

    private static HttpRequestMessage NewHttpRequest(string verb, string uri, string jsonBody)
    {
        var stringContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        return new HttpRequestMessage
        {
            Method = new HttpMethod(verb),
            RequestUri = new Uri(uri),
            Content = jsonBody is not null ? stringContent : null
        };
    }

    private string GetParamValue(HttpClientParam httpClientParam, ICollection<MetadataDto> requestMetadata, string templateContent, string requestToDestination)
    {
        string value = string.Empty;

        if (httpClientParam.HasStaticValue == true)
            value = httpClientParam.StaticValue;

        if (httpClientParam.ReadValueFrom == HttpClientParamValueReader.TemplateContent)
            value = templateContent;
        
        if (httpClientParam.ReadValueFrom == HttpClientParamValueReader.RequestToDestination)
            value = requestToDestination;
        
        if (httpClientParam.ReadValueFrom == HttpClientParamValueReader.RequestMetadata)
        {
            value = requestMetadata
                .Where(x => x.Key == httpClientParam.Name)
                .Select(x => x.Value)
                .FirstOrDefault()!;
        }
        return value;
    }

    private static void SetQueryString(Dictionary<string, string> queryString, string name, string value)
        => queryString.Add(name, value);
    
    private void SetHeader(string name, string value)
        => _httpClient.DefaultRequestHeaders.Add(name, value);
    
    private static void SetRoute(string uri, string name, string value)
        => uri = uri.Replace($"{{{name}}}", value);
    
    private static void SetMetadataToJsonBodyDefinition(ICollection<MetadataDto> metadata, string name, string value)
        => metadata.Add(new MetadataDto { Key = name, Value = value });

    private string GetJson(JsonBody jsonBody, ICollection<MetadataDto> requestMetadata)
    {
        if (jsonBody is null)
            return string.Empty;
        
        var body = _mapper.Map<Application.Features.Providers.Libraries.JSONParser.JsonBody>(jsonBody);
        var metadata = _mapper.Map<List<Application.Features.Providers.Libraries.JSONParser.Metadata>>(requestMetadata);
        body.Metadata = metadata;

        var json = body
            .Prepare()
            .Transform();
        
        return json;
    }
}