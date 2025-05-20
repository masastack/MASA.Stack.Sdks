namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class ClaimValueModel
{
    public ClaimValueModel(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public string Key { get; set; } = "";

    public string Value { get; set; } = "";
}
