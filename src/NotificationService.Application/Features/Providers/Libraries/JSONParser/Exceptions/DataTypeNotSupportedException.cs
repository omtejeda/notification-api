namespace NotificationService.Application.Features.Providers.Libraries.JsonParser.Exceptions;

public class DataTypeNotSupportedException : Exception
{
    public DataTypeNotSupportedException(DataType dataType) : base($"The data type specified is not supported: {dataType}") { }
    public DataTypeNotSupportedException(string message) : base(message) { }
    public DataTypeNotSupportedException(string message, Exception inner) : base(message, inner) { }
}