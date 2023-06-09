using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using NotificationService.Exceptions;
using NotificationService.Dtos;
using NotificationService.Entities;

namespace NotificationService.Utils
{
    public static class HttpUtil
    {
        private static List<string> GetVerbsAllowed() => new List<string>{"GET", "POST", "PUT", "PATCH"};

        public static string GetFullPath(string host, string uri, IDictionary<string, string> queryString = null)
        {
            var pathSeparator = Path.AltDirectorySeparatorChar;
            var fullPath = string.Empty;

            if (!host.EndsWith(pathSeparator) && !uri.StartsWith(pathSeparator))
                fullPath = host + pathSeparator + uri;

            else if (host.EndsWith(pathSeparator) && uri.StartsWith(pathSeparator))
                fullPath = host + uri.TrimStart(pathSeparator);

            else
                fullPath = host + uri;
            
            if (queryString != null)
                fullPath = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(fullPath, queryString);

            return fullPath;
        }
        public static void CheckHTTPClientSettings(string host, string uri, string verb)
            => CheckHTTPClientSettings(new HttpClientSettingDTO { Host = host, Uri = uri, Verb = verb });

        public static void CheckHTTPClientSettings(HttpClientSettingDTO settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Host)) throw new RuleValidationException($"A value for {nameof(settings.Host)} is required");
            if (string.IsNullOrWhiteSpace(settings.Uri)) throw new RuleValidationException($"A value for {nameof(settings.Uri)} is required");
            if (string.IsNullOrWhiteSpace(settings.Verb)) throw new RuleValidationException($"A value for {nameof(settings.Verb)} is required");
            if (!GetVerbsAllowed().Any(x => x == settings.Verb)) throw new RuleValidationException($"HTTP verb: {settings.Verb} not allowed");

            foreach (var param in settings?.Params)
            {
                
                try { Enum.Parse<HttpClientParamType>(param.Type, true); }
                catch (Exception) { throw new RuleValidationException($"Value not valid [{param.Type}]"); }

                try { Enum.Parse<HttpClientParamValueReader>(param.ReadValueFrom, true); }
                catch (Exception) { throw new RuleValidationException($"Value not valid [{param.ReadValueFrom}]"); }
            }
        }
    }
}