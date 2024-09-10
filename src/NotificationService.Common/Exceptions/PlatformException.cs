namespace NotificationService.Common.Exceptions;

public class PlatformException : RuleValidationException
{
    public PlatformException() {}
    public PlatformException(string message) : base(message) {}
}