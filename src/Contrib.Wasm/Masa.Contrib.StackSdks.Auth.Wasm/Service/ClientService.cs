// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Auth.Wasm.Service;

public class ClientService : IClientService
{
    readonly ICaller _caller;

    const string _party = "api/sso/client/";

    public ClientService(ICaller caller)
    {
        _caller = caller;
    }

    public async Task<List<ClientSelectModel>> GetClientSelectAsync()
    {
        var requestUri = $"{_party}GetClientSelect";
        return await _caller.GetAsync<List<ClientSelectModel>>(requestUri) ?? new();
    }
}
