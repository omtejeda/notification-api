namespace NotificationService.Core.Common.Enums
{
    public enum ErrorCode
    {
        OK = 0,
        Error = 1,
        Warning = 2,
        AccessDenied = 3,
        ValidationError = 4,

        EmailOK = 10,
        EmailNotSent = 11,
        EmailPending = 12,


        TemplateOK = 20,
        TemplateDoesNotExist = 21,
        TemplateInvalid = 22,

        PlatformOK = 30,
        PlatformNotActive = 31,
        PlatformInvalid = 32,

        HttpRequestOK = 40,
        HttpRequestNotSent = 41,
        HttpRequestPending = 42
    }
}