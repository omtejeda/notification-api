namespace NotificationService.Common.Utils
{
    public static class AppUtil
    {
        public static int? Gmt
            => GetIntEnvironmentVariable("GMT_TIMEZONE");
        public static DateTime CurrentDate
            => DateTime.Now.AddHours(Gmt.GetValueOrDefault());

        private static int? GetIntEnvironmentVariable(string name)
        {
            var gotten = int.TryParse(Environment.GetEnvironmentVariable(name), out var value);
            return gotten ? value : default(int?);
        }
    }
}