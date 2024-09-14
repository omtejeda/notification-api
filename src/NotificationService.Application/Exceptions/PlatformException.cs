namespace NotificationService.Application.Exceptions;

public class PlatformException : RuleValidationException
{
    public PlatformException() {}
    public PlatformException(string message) : base(message) {}
}