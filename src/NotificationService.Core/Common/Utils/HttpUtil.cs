using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using NotificationService.Core.Common.Exceptions;
using NotificationService.Common.Dtos;
using NotificationService.Common.Entities;
using NotificationService.Common.Resources;

namespace NotificationService.Core.Common.Utils
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
            
            if (queryString is not null)
                fullPath = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(fullPath, queryString);

            return fullPath;
        }
        public static void CheckHTTPClientSettings(string host, string uri, string verb)
            => CheckHTTPClientSettings(new HttpClientSettingDto { Host = host, Uri = uri, Verb = verb });

        public static void CheckHTTPClientSettings(HttpClientSettingDto settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Host)) throw new RuleValidationException(string.Format(Messages.RequiredValue, nameof(settings.Host)));
            if (string.IsNullOrWhiteSpace(settings.Uri)) throw new RuleValidationException(string.Format(Messages.RequiredValue, nameof(settings.Uri)));
            if (string.IsNullOrWhiteSpace(settings.Verb)) throw new RuleValidationException(string.Format(Messages.RequiredValue, nameof(settings.Verb)));
            if (!GetVerbsAllowed().Any(x => x == settings.Verb)) throw new RuleValidationException(string.Format(Messages.HttpVerbNotAllowed, settings.Verb));

            foreach (var param in settings?.Params)
            {
                
                try { Enum.Parse<HttpClientParamType>(param.Type, true); }
                catch (Exception) { throw new RuleValidationException(string.Format(Messages.ValueNotValid, param.Type)); }

                try { Enum.Parse<HttpClientParamValueReader>(param.ReadValueFrom, true); }
                catch (Exception) { throw new RuleValidationException(string.Format(Messages.ValueNotValid, param.ReadValueFrom)); }
            }
        }
    }
}