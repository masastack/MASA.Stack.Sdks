// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Sapp.Service;

public class GlobalNavService : IGlobalNavService
{
    private const string GLOBAL_NAV_ROUTE_PREFIX = "api/global-nav";

    private readonly ICaller _caller;

    public GlobalNavService(ICaller caller)
    {
        _caller = caller;
    }

    public async Task<List<GlobalNavigationAppDto>> GetGlobalNavigationsByClientIdAsync(string clientId)
    {
        var requestUri = $"{GLOBAL_NAV_ROUTE_PREFIX}/by-client-id/{clientId}";
        var result = await _caller.GetAsync<List<GlobalNavigationAppDto>>(requestUri);
        return result ?? new();
    }

    public async Task<List<AppEntryDto>> GetVisibleAppEntriesByClientIdAsync(string clientId)
    {
        var requestUri = $"{GLOBAL_NAV_ROUTE_PREFIX}/app-entries/by-client-id/{clientId}";
        var result = await _caller.GetAsync<List<AppEntryDto>>(requestUri);
        return result ?? new();
    }
}
