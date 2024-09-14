namespace NotificationService.Application.Exceptions;

public class ProviderException : RuleValidationException
{
    public ProviderException() {}
    public ProviderException(string message) : base(message) {}
}