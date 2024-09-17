/// <summary>
/// Represents a JSON key or property with properties for the property name, 
/// data type, whether it is required, child keys, its value, and index if needed when array.
/// </summary>
using Newtonsoft.Json;
namespace NotificationService.Application.Features.Providers.Libraries.JSONParser;
public class JsonKey
{
    public string PropertyName { get; set; } = string.Empty;
    public DataType DataType { get; set; }
    public bool IsRequired { get; set; }
    public List<JsonKey> Childs { get; set; } = new();
    public object? Value { get; set; }
    public int Index { get; set; }

    public JsonKey Clone()
    {
        var copy = new JsonKey
        {
            PropertyName = PropertyName,
            DataType = DataType,
            IsRequired = IsRequired,
            Childs = Childs,
            Value = Value,
            Index = Index
        };

        var serialized = JsonConvert.SerializeObject(copy);
        return JsonConvert.DeserializeObject<JsonKey>(serialized)!;
    }
}