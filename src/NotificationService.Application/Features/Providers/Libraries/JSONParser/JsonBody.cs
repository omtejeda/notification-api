/// <summary>
/// Represents a JSON body or object with a root property name and a list of JsonKey objects that define its structure.
/// </summary>
using Newtonsoft.Json;
using NotificationService.Application.Features.Providers.Libraries.JSONParser.Exceptions;
namespace NotificationService.Application.Features.Providers.Libraries.JSONParser;

public class JsonBody
{
    private DataType _rootIs;
    public DataType RootIs
    {
        get => _rootIs;
        set
        {
            if (IsSimpleValue(value))
                throw new DataTypeNotSupportedException(value);
            
            _rootIs = value;
        }
    }
    public List<JsonKey> Definition { get; set; } = [];
    public List<Metadata> Metadata { get; set; } = [];

    public JsonBody Prepare()
    {
        AddArrayItemsGivenMetadata();
        Alter();

        return this;
    }


    /// <summary>
    /// Checks whether a given value is simple.
    /// </summary>
    /// <param name="dataType"></param>
    /// <returns></returns>
    private static bool IsSimpleValue(DataType dataType)
        => dataType != DataType.Object &&
           dataType != DataType.Array;

    /// <summary>
    /// Creates a simple value. A simple value is a value type which is not either an object or an array.
    /// </summary>
    /// <param name="dataType"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private object? CreateSimpleValue(DataType dataType, object? value = null)
    {
        if (dataType == DataType.Number)
            return Metadata.Any() ? (value is not null ? Convert.ToInt32(value!) : value!)  : default(int);

        if (dataType == DataType.Date)
            return Metadata.Any() ? value ??= null : default(DateTime);

        if (dataType == DataType.Boolean)
            return Metadata.Any() ? value ??= null : default(bool);

        return Metadata.Any() ? value ??= null : dataType.ToString().ToLower();
    }

    /// <summary>
    /// Creates an array based on keys collection. Every key might be an object property. Can hold different data type values.
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    private object CreateObject(ICollection<JsonKey> keys)
    {
        var customObject = new Dictionary<string, object>();
        
        foreach (var key in keys)
        {
            if (key.DataType == DataType.Array)
            {
                var array = CreateArray(key.Childs);
                customObject[key.PropertyName] = array;
            }

            if (key.DataType == DataType.Object)
            {
                var @object = CreateObject(key.Childs);
                customObject[key.PropertyName] = @object;
            }

            if (IsSimpleValue(key.DataType))
            {
                var simpleValue = CreateSimpleValue(key.DataType, key.Value);
                
                if (simpleValue is not null)
                    customObject[key.PropertyName] = simpleValue;
            }

            if (key.DataType == DataType.DynamicObject)
            {
                var dynamicValue = Metadata.FirstOrDefault(x => x.Key == key.PropertyName);
                
                if (dynamicValue is not null)
                    customObject[key.PropertyName] = GetObjectFromJson(dynamicValue.Value);
            }
        }
        return customObject;
    }

    private static object GetObjectFromJson(string json)
    {
        return JsonConvert.DeserializeObject<object>(json) ?? new();
    }
    
    /// <summary>
    /// Creates an array based on keys collection
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    private object CreateArray(ICollection<JsonKey> keys)
    {
        var array = new List<object>();

        foreach (var key in keys)
        {
            if (IsSimpleValue(key.DataType))
            {
                var simpleValue = CreateSimpleValue(key.DataType, key.Value);
                if (simpleValue is not null)
                {
                    array.Add(simpleValue);
                    
                    if (key.Value is null)
                        break;
                }
            }

            if (key.DataType == DataType.Object)
            {
                var @object = CreateObject(key.Childs);
                var dictionary = (Dictionary<string, object>) @object;
                var str = dictionary.FirstOrDefault().Value as string;

                if (!string.IsNullOrEmpty(str))
                {
                    array.Add(@object);
                }
            }
        }
        return array;
    }

    /// <summary>
    /// Creates a JSON representation based on the definition. 
    /// </summary>
    /// <returns></returns>
    public string Transform()
    {        
        var obj = new object();
        
        if (RootIs == DataType.Object)
            obj = CreateObject(Definition);

        if (RootIs == DataType.Array)
            obj = CreateArray(Definition);

        return JsonConvert.SerializeObject(obj);
    }

    /// <summary>
    /// Adds extra items to arrays based on metadata. In definition arrays only hold a single value, at runtime it might hold N values though.
    /// Needs to be called after metadata is added and before Alter method starts its execution.
    /// </summary>
    private void AddArrayItemsGivenMetadata()
    {
        var arrays = Metadata.Where(x => x.Key.Contains(']'));
        
        if (!arrays.Any())
            return;

        var propertiesWithIndex = arrays
            .Select(x => x.Key
                    .Split(']')
                    .FirstOrDefault() + ']')
            .Distinct();
        
        var propertiesIndexless = propertiesWithIndex
            .Select(x => x
                    .Split('[')
                    .FirstOrDefault())
            .Distinct();

        foreach (var propertyName in propertiesIndexless)
        {
            if (string.IsNullOrEmpty(propertyName))
                continue;

            var count = propertiesWithIndex.Count(x => x.StartsWith($"{propertyName}["));
            var jsonKey = Definition.FirstOrDefault(x => x.PropertyName.Equals(propertyName) && x.DataType == DataType.Array);
            
            var firstChild = jsonKey?.Childs.FirstOrDefault();
            if (firstChild is null)
                continue;
            
            for (int i = 1; i < count; i++)
            {
                var clonedKey = firstChild.Clone();
                clonedKey.Index = i;
                jsonKey!.Childs.Add(clonedKey);
            }
        }
    }

    /// <summary>
    /// Populate properties values' once the metadata has been supplied
    /// Beware when calling this method since it makes use of recursion internally.
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="parentName"></param>
    /// <param name="parentIsArray"></param>
    /// <param name="parentDataType"></param>
    /// <exception cref="PropertyRequiredException"></exception>
    private void Alter(List<JsonKey>? keys = null, string? parentName = null, bool parentIsArray = false, DataType parentDataType = DataType.Undefined)
    {
        var definition = keys ?? Definition;

        foreach (var def in definition)
        {
            if (def.DataType == DataType.Array)
            {
                Alter(keys: def.Childs, 
                    parentName: def.PropertyName, 
                    parentIsArray: true,
                    parentDataType: def.DataType);
            }
            else if (def.DataType == DataType.Object)
            {
                var indexOrPropertyName = string.IsNullOrEmpty(def.PropertyName) ? $"[{def.Index}]" : $".{def.PropertyName}";
                var customParentName = parentIsArray ? $"{parentName}{indexOrPropertyName}" : def.PropertyName;
                
                Alter(keys: def.Childs, 
                    parentName: customParentName, 
                    parentIsArray: parentIsArray,
                    parentDataType: def.DataType);
            }
            else
            {
                var arrayParentName = $"{parentName}[{def.Index}]";
                var objectParentName = $"{parentName}.{def.PropertyName}";

                var name = string.IsNullOrEmpty(parentName) ? 
                    def.PropertyName : 
                    parentDataType == DataType.Array ? arrayParentName : objectParentName;
                
                var metadata = Metadata.FirstOrDefault(x => x.Key.Equals(name));

                if (metadata is null && def.IsRequired)
                    throw new PropertyRequiredException($"Missing value for [{name}] in metadata. It's required.");
                
                if (metadata is null)
                    continue;
                
                def.Value = metadata.Value;
            }
        }
    }
}