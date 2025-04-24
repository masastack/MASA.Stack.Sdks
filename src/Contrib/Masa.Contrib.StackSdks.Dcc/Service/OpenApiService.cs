namespace Masa.Contrib.StackSdks.Dcc.Service;

internal class OpenApiService : IOpenApiService
{
    readonly ICaller _caller;

    public OpenApiService(ICallerFactory callerFactory)
    {
        _caller = callerFactory.Create(DCC_CONFIGAPI_CLIENT_NAME);
        if (_caller == null)
            throw new Exception($"please inject dcc configapi client by UseDcc");
    }

    public Task<Dictionary<string, string>> GetI18NConfigAsync(string culture, string environment = "", string cluster = "")
    {
        throw new NotSupportedException("GetI18NConfigAsync is not supported in server mode");
    }

    public async Task<OssSecurityTokenDto> GetOssSecurityTokenAsync()
    {
        var requestUri = $"open-api/oss-token";
        var result = await _caller.GetAsync<OssSecurityTokenDto>(requestUri);
        return result!;
    }

    public Task<Dictionary<string, string>> GetStackConfigAsync(string environment = "", string cluster = "")
    {
        throw new NotSupportedException("GetI18NConfigAsync is not supported in server mode");
    }
}
