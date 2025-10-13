namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public enum StatementEffect
{
    Deny = 0,
    Allow = 1
}

public class ControlPolicyModel
{
    public Guid Id { get; set; }

    /// <summary>
    /// 策略名称，便于识别和管理
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 策略效果：Allow 或 Deny
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StatementEffect Effect { get; set; } = StatementEffect.Deny;

    /// <summary>
    /// 策略优先级，数值越大优先级越高
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 是否启用此策略
    /// </summary>
    public bool Enabled { get; set; } = true;

    [JsonConverter(typeof(ActionIdentifierConverter))]
    public List<ActionIdentifierModel> Actions { get; set; } = new();

    [JsonConverter(typeof(ResourceIdentifierConverter))]
    public List<ResourceIdentifierModel> Resources { get; set; } = new();
}

public class ActionIdentifierModel
{
    public string Resource { get; set; } = "*";

    public string Type { get; set; } = "*";

    public string Operation { get; set; } = "*";

    /// <summary>
    /// 从字符串解析ActionIdentifier，格式为 Resource:Type:Operation
    /// </summary>
    /// <param name="actionName">操作标识符字符串</param>
    public ActionIdentifierModel(string? actionName = null)
    {
        if (string.IsNullOrWhiteSpace(actionName))
        {
            return; // 使用默认值 "*"
        }

        var parts = actionName.Split(':', StringSplitOptions.None);

        if (parts.Length >= 1 && !string.IsNullOrEmpty(parts[0]))
            Resource = parts[0];

        if (parts.Length >= 2 && !string.IsNullOrEmpty(parts[1]))
            Type = parts[1];

        if (parts.Length >= 3 && !string.IsNullOrEmpty(parts[2]))
            Operation = parts[2];
    }

    public ActionIdentifierModel() { }

    public override string ToString()
    {
        return $"{Resource}:{Type}:{Operation}";
    }
}

public class ActionIdentifierConverter : JsonConverter<List<ActionIdentifierModel>>
{
    public override List<ActionIdentifierModel> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var actionName = reader.GetString();
            return new List<ActionIdentifierModel> { new ActionIdentifierModel(actionName) };
        }
        else if (reader.TokenType == JsonTokenType.StartArray)
        {
            var actionNames = JsonSerializer.Deserialize<List<string>>(ref reader, options);
            return actionNames?.ConvertAll(actionName => new ActionIdentifierModel(actionName)) ?? new List<ActionIdentifierModel>();
        }
        throw new JsonException("Unexpected token type.");
    }

    public override void Write(Utf8JsonWriter writer, List<ActionIdentifierModel> value, JsonSerializerOptions options)
    {
        if (value == null || value.Count == 0)
        {
            writer.WriteStringValue("*");
        }
        else if (value.Count == 1)
        {
            writer.WriteStringValue(value[0].ToString());
        }
        else
        {
            var actionNames = value?.Select(action => action.ToString()) ?? new List<string>();
            JsonSerializer.Serialize(writer, actionNames, options);
        }
    }
}

public class ResourceIdentifierModel
{
    public string Service { get; set; } = "*";

    public string Region { get; set; } = "*";

    public string Identifier { get; set; } = "*";

    /// <summary>
    /// 从字符串解析Resource，格式为 Service:Region:Identifier
    /// </summary>
    /// <param name="resource">资源标识符字符串</param>
    public ResourceIdentifierModel(string? resource = null)
    {
        if (string.IsNullOrWhiteSpace(resource))
        {
            return; // 使用默认值 "*"
        }

        var parts = resource.Split(':', StringSplitOptions.None);

        if (parts.Length >= 1 && !string.IsNullOrEmpty(parts[0]))
            Service = parts[0];

        if (parts.Length >= 2 && !string.IsNullOrEmpty(parts[1]))
            Region = parts[1];

        if (parts.Length >= 3 && !string.IsNullOrEmpty(parts[2]))
            Identifier = parts[2];
    }

    public ResourceIdentifierModel() { }

    public override string ToString()
    {
        return $"{Service}:{Region}:{Identifier}";
    }
}

/// <summary>
/// Resource的JSON转换器
/// </summary>
public class ResourceIdentifierConverter : JsonConverter<List<ResourceIdentifierModel>>
{
    public override List<ResourceIdentifierModel> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var resource = reader.GetString();
            return new List<ResourceIdentifierModel> { new ResourceIdentifierModel(resource) };
        }
        else if (reader.TokenType == JsonTokenType.StartArray)
        {
            var resources = JsonSerializer.Deserialize<List<string>>(ref reader, options);
            return resources?.ConvertAll(resource => new ResourceIdentifierModel(resource)) ?? new List<ResourceIdentifierModel>();
        }
        throw new JsonException("Unexpected token type.");
    }

    public override void Write(Utf8JsonWriter writer, List<ResourceIdentifierModel> value, JsonSerializerOptions options)
    {
        if (value == null || value.Count == 0)
        {
            writer.WriteStringValue("*");
        }
        else if (value.Count == 1)
        {
            writer.WriteStringValue(value[0].ToString());
        }
        else
        {
            var resources = value?.Select(action => action.ToString()) ?? new List<string>();
            JsonSerializer.Serialize(writer, resources, options);
        }
    }
}
