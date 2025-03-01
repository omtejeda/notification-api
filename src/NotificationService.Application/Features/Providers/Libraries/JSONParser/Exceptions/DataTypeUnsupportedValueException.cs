namespace NotificationService.Application.Features.Providers.Libraries.JsonParser.Exceptions;
public class DataTypeUnsupportedValueException : Exception
{
    public DataTypeUnsupportedValueException() { }
    public DataTypeUnsupportedValueException(string message) : base(message) { }
    public DataTypeUnsupportedValueException(string message, Exception inner) : base(message, inner) { }
}