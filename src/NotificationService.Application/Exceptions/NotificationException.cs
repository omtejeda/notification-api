namespace NotificationService.Application.Exceptions;

public class NotificationException : RuleValidationException
{
    public NotificationException() {}
    public NotificationException(string message) : base(message) {}
}