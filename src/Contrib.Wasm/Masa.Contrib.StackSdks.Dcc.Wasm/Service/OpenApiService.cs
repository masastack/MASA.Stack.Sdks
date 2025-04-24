namespace Masa.Contrib.StackSdks.Dcc.Service;

internal class OpenApiService : IOpenApiService
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

    public async Task<Dictionary<string, string>> GetStackConfigAsync(string environment, string cluster)
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

        var requestUri = $"open-api/releasing/{environment}/{cluster}/stack-config";
        var result = await _caller.GetAsync<Dictionary<string, string>>(requestUri);

        return result ?? new();
    }

    public async Task<Dictionary<string, string>> GetI18NConfigAsync(string culture, string environment, string cluster)
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

        var requestUri = $"open-api/releasing/{environment}/{cluster}/i18n/{culture}";
        var result = await _caller.GetAsync<Dictionary<string, string>>(requestUri);

        return result ?? new();
    }

    public async Task<OssSecurityTokenDto> GetOssSecurityTokenAsync()
    {
        var requestUri = $"open-api/oss-token";
        var result = await _caller.GetAsync<OssSecurityTokenDto>(requestUri);
        return result!;
    }
}
