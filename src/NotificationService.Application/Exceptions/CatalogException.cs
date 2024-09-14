namespace NotificationService.Application.Exceptions;

public class CatalogException : RuleValidationException
{
    public CatalogException() {}
    public CatalogException(string message) : base(message) {}
}