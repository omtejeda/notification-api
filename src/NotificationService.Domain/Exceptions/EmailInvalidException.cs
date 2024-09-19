namespace NotificationService.Domain.Exceptions;

public class EmailInvalidException : DomainException
{
    public EmailInvalidException() {}
    public EmailInvalidException(string message) : base(message) {}
    public EmailInvalidException(string message, Exception innerException) : base(message, innerException) {}
}