// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Sapp.Service;

public class ModuleService : IModuleService
{
    private const string MODULE_ROUTE_PREFIX = "api/module";

    private readonly ICaller _caller;

    public ModuleService(ICaller caller)
    {
        _caller = caller;
    }

    public Task<ModuleDetailDto?> GetByPmIdentityAsync(string pmIdentity)
    {
        var requestUri = $"{MODULE_ROUTE_PREFIX}/byPmIdentity";
        return _caller.GetAsync<ModuleDetailDto?>(requestUri, new { pmIdentity });
    }
}
