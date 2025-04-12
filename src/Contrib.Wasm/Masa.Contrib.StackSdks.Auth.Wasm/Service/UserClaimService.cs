// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Auth.Wasm.Service;

public class UserClaimService : IUserClaimService
{
    readonly ICaller _caller;

    const string _party = "api/sso/userClaim/";

    public UserClaimService(ICaller caller)
    {
        _caller = caller;
    }

    public async Task<List<UserClaimSelectModel>> GetSelectAsync(string? search)
    {
        var requestUri = $"{_party}GetSelect";
        return await _caller.GetAsync<object, List<UserClaimSelectModel>>(requestUri, new { search }) ?? new();
    }
}
