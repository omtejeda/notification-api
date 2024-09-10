namespace NotificationService.Common.Exceptions;

public class ProviderException : RuleValidationException
{
    public ProviderException() {}
    public ProviderException(string message) : base(message) {}
}