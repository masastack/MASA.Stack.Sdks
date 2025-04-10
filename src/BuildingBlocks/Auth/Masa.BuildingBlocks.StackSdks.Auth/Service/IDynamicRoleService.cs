// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Auth.Service;

public interface IDynamicRoleService
{
    Task<AuthPaginationModel<DynamicRoleModel>> GetPageAsync(GetDynamicRoleInputModel input);

    Task<DynamicRoleModel> GetAsync(Guid id);

    Task CreateAsync(DynamicRoleUpsertModel input);

    Task UpdateAsync(Guid id, DynamicRoleUpsertModel input);

    Task DeleteAsync(Guid id);
}
