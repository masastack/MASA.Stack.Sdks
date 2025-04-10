// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Auth.Wasm.Service;

public class RoleService : IRoleService
{
    readonly ICaller _caller;

    const string _party = "api/role/";

    public RoleService(ICaller caller)
    {
        _caller = caller;
    }

    public async Task<AuthPaginationModel<RoleModel>> GetListAsync(GetRolesModel input)
    {
        var requestUri = $"{_party}GetList";
        return await _caller.GetAsync<GetRolesModel, AuthPaginationModel<RoleModel>>(requestUri, input) ?? new();
    }

    public async Task<RoleDetailModel?> GetDetailAsync(Guid id)
    {
        var requestUri = $"{_party}GetDetail";
        return await _caller.GetAsync<object, RoleDetailModel>(requestUri, new { id });
    }

    public async Task AddAsync(AddRoleModel input)
    {
        var requestUri = $"{_party}Add";
        await _caller.PostAsync(requestUri, input);
    }

    public async Task UpdateAsync(UpdateRoleModel input)
    {
        var requestUri = $"{_party}Update";
        await _caller.PutAsync(requestUri, input);
    }

    public async Task RemoveAsync(Guid id)
    {
        var requestUri = $"{_party}Remove";
        await _caller.DeleteAsync(requestUri, new { id });
    }

    public async Task<List<RoleSelectModel>> GetSelectForUserAsync(Guid userId)
    {
        var requestUri = $"{_party}GetSelectForUser";
        return await _caller.GetAsync<object, List<RoleSelectModel>>(requestUri, new { userId }) ?? new();
    }
}
