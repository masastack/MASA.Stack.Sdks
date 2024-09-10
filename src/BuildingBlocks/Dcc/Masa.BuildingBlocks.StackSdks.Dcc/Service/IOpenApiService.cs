namespace Masa.BuildingBlocks.StackSdks.Dcc.Service;

public interface IOpenApiService
{
    Task<T> GetPublicConfigAsync<T>(string configObject, string environment = "", string cluster = "") where T : class, new();
}
