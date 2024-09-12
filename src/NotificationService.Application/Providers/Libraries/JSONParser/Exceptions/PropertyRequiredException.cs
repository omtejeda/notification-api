using System;
namespace NotificationService.Application.Providers.Libraries.JSONParser.Exceptions;
public class PropertyRequiredException : Exception
{
    public PropertyRequiredException() { }
    public PropertyRequiredException(string message) : base(message) { }
    public PropertyRequiredException(string message, Exception inner) : base(message, inner) { }
}