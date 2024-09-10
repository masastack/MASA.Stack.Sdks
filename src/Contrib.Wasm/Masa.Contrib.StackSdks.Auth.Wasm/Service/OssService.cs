namespace Masa.Contrib.StackSdks.Auth.Service;

public class OssService : IOssService
{
    readonly ICaller _caller;

    const string PARTY = "api/oss/";

    public OssService(ICaller caller)
    {
        _caller = caller;
    }

    public async Task<SecurityTokenModel> GetSecurityTokenAsync()
    {
        var requestUri = $"{PARTY}GetSecurityToken";
        return await _caller.GetAsync<SecurityTokenModel>(requestUri) ?? new();
    }
}
