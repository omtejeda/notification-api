namespace NotificationService.Common.Enums
{
    public enum ResultCode
    {
        OK = 0,
        Error = 1,
        Warning = 2,
        AccessDenied = 3,
        ValidationError = 4,
        EmailNotSent = 11,
        HttpRequestOK = 40,
        HttpRequestNotSent = 41
    }
}