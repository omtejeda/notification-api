namespace NotificationService.Common.Utils;

public static class AppUtil
{
    public static int? Gmt
        => GetIntEnvironmentVariable("GMT_TIMEZONE");
    
    public static DateTime CurrentDate
        => DateTime.Now.AddHours(Gmt.GetValueOrDefault());

    private static int GetIntEnvironmentVariable(string name)
    {
        return int.TryParse(Environment.GetEnvironmentVariable(name), out var value)
        ? value
        : default;
    }
}