// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Auth.Service;

public class ThirdPartyIdpService : IThirdPartyIdpService
{
    readonly ICaller _caller;

    public ThirdPartyIdpService(ICaller caller)
    {
        _caller = caller;
    }

    public async Task<List<ThirdPartyIdpModel>> GetAllAsync()
    {
        var requestUri = $"api/thirdPartyIdp/getAll";
        return await _caller.GetAsync<List<ThirdPartyIdpModel>>(requestUri) ?? new();
    }

    public Task<List<ThirdPartyIdpModel>> GetAllFromCacheAsync()
    {
        throw new NotImplementedException();
    }

    public Task<LdapOptionsModel?> GetLdapOptionsAsync(string scheme)
    {
        var requestUri = $"api/thirdPartyIdp/ldapOptions";
        return _caller.GetAsync<LdapOptionsModel>(requestUri, new { scheme });
    }

    public async Task<List<ThirdPartyIdpSelectModel>> GetSelectAsync(string? search, bool includeLdap)
    {
        var requestUri = $"api/thirdPartyIdp/getSelect";
        return await _caller.GetAsync<List<ThirdPartyIdpSelectModel>>(requestUri, new { search, includeLdap }) ?? new();
    }
}
