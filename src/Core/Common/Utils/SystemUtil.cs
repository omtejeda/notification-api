using System;
namespace NotificationService.Core.Common.Utils
{
    public static class SystemUtil
    {
        public static string GetEnvironment()
            => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToString();
        public static bool IsProduction ()
            => GetEnvironment().ToUpper() == "PRODUCTION";
        public static int? GetGMT()
            => GetIntEnvironmentVariable("GMT_TIMEZONE");
        public static DateTime GetSystemDate()
            => DateTime.Now.AddHours(GetGMT().GetValueOrDefault());
        public static int? GetLimitPageSize()
            => GetIntEnvironmentVariable("LIMIT_PAGE_SIZE");

        private static int? GetIntEnvironmentVariable(string name)
        {
            var gotten = int.TryParse(Environment.GetEnvironmentVariable(name), out var value);
            return gotten ? value : default(int?);
        }
    }
}