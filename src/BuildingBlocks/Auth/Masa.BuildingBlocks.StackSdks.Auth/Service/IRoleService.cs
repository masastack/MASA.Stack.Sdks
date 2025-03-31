// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Auth.Service;

public interface IRoleService
{
    Task<AuthPaginationModel<RoleModel>> GetListAsync(GetRolesModel input);

    Task<RoleDetailModel?> GetDetailAsync(Guid id);

    Task AddAsync(AddRoleModel input);

    Task UpdateAsync(UpdateRoleModel input);

    Task RemoveAsync(Guid id);
}
