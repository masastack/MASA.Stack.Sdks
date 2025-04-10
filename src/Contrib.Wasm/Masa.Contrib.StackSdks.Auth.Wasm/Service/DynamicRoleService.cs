// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Auth.Wasm.Service;

public class DynamicRoleService : IDynamicRoleService
{
    readonly ICaller _caller;

    const string _party = "api/dynamic-role";

    public DynamicRoleService(ICaller caller)
    {
        _caller = caller;
    }

    public async Task<AuthPaginationModel<DynamicRoleModel>> GetPageAsync(GetDynamicRoleInputModel input)
    {
        var requestUri = $"{_party}";
        return await _caller.GetAsync<GetDynamicRoleInputModel, AuthPaginationModel<DynamicRoleModel>>(requestUri, input) ?? new();
    }

    public async Task<DynamicRoleModel> GetAsync(Guid id)
    {
        var requestUri = $"{_party}/{id}";
        return await _caller.GetAsync<DynamicRoleModel>(requestUri) ?? new();
    }

    public async Task CreateAsync(DynamicRoleUpsertModel input)
    {
        var requestUri = $"{_party}";
        await _caller.PostAsync(requestUri, input);
    }

    public async Task UpdateAsync(Guid id, DynamicRoleUpsertModel input)
    {
        var requestUri = $"{_party}/{id}";
        await _caller.PutAsync(requestUri, input);
    }

    public async Task DeleteAsync(Guid id)
    {
        var requestUri = $"{_party}/{id}";
        await _caller.DeleteAsync(requestUri, null);
    }
}