namespace Masa.BuildingBlocks.StackSdks.Dcc.Service;

public interface IOpenApiService
{
    Task<Dictionary<string, string>> GetStackConfigAsync(string environment = "", string cluster = "");

    Task<Dictionary<string, string>> GetI18NConfigAsync(string culture, string environment = "", string cluster = "");
}
