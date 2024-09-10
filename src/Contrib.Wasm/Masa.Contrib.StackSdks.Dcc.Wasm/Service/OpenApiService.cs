namespace Masa.Contrib.StackSdks.Dcc.Service;

public class OpenApiService : IOpenApiService
{
    private readonly ICaller _caller;
    private readonly IMultiEnvironmentUserContext _environmentUserContext;
    private readonly IMasaStackConfig _masaStackConfig;

    public OpenApiService(ICaller caller, IMultiEnvironmentUserContext environmentUserContext, IMasaStackConfig masaStackConfig)
    {
        _caller = caller;
        _environmentUserContext = environmentUserContext;
        _masaStackConfig = masaStackConfig;
    }

    public async Task<T> GetPublicConfigAsync<T>(string configObject, string environment, string cluster) where T : class, new()
    {
        if (environment.IsNullOrEmpty())
        {
            environment = _environmentUserContext.Environment ?? string.Empty;
        }
        if (environment.IsNullOrEmpty())
        {
            environment = _masaStackConfig.Environment;
        }
        if (cluster.IsNullOrEmpty())
        {
            cluster = Configs.DEFAULT_CLUSTER;
        }
        var requestUri = $"open-api/releasing/{environment}/{cluster}/publicConfig/{configObject}";
        var result = await _caller.GetAsync<T>(requestUri);

        return result ?? new T();
    }
}
