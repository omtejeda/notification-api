namespace NotificationService.Common.Exceptions;

public class TemplateException : RuleValidationException
{
    public TemplateException() {}
    public TemplateException(string message) : base(message) {}
}