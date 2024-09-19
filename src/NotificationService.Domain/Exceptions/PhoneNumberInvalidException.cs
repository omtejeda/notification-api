namespace NotificationService.Domain.Exceptions;

public class PhoneNumberInvalidException : DomainException
{
    public PhoneNumberInvalidException() {}
    public PhoneNumberInvalidException(string message) : base(message) {}
    public PhoneNumberInvalidException(string message, Exception innerException) : base(message, innerException) {}
}