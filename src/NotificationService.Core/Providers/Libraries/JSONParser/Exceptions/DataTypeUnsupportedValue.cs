using System;
namespace NotificationService.Core.Providers.Libraries.JSONParser.Exceptions;
public class DataTypeUnsupportedValue : Exception
{
    public DataTypeUnsupportedValue() { }
    public DataTypeUnsupportedValue(string message) : base(message) { }
    public DataTypeUnsupportedValue(string message, Exception inner) : base(message, inner) { }
}