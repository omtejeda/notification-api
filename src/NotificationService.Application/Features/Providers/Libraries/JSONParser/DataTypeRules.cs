using NotificationService.Application.Features.Providers.Libraries.JSONParser.Exceptions;

namespace NotificationService.Application.Features.Providers.Libraries.JSONParser;

public class DataTypeRules
{
    public string GetValueOrThrow(DataType dataType, string value)
    {
        var (isValid, finalValue) = IsValidValue(dataType, value);
        if (!isValid)
            throw new DataTypeUnsupportedValue($"Value {value} not supported for data type {dataType}");
        
        return finalValue as string;
    }

    private (bool, object) IsValidValue(DataType dataType, string value)
    {
        if (dataType == DataType.Date)
            return CheckIfValid(() => Convert.ToDateTime(value));

        if (dataType == DataType.Boolean)
            return CheckIfValid(() => Convert.ToBoolean(value));

        if (dataType == DataType.Number)
            return CheckIfValid(() => Convert.ToDecimal(value));
        
        return (false, null);
    }

    private (bool, object) CheckIfValid(Func<object> action)
    {
        bool isValid = true;
        object value = new();
        
        try
        {
            value = action.Invoke();
        }
        catch
        {
            isValid = false; 
        }
        return (isValid, value);
    }
}