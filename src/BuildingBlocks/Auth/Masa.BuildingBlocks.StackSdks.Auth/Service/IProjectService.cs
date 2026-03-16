// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Auth.Service;

public interface IProjectService
{
    Task<List<ProjectModel>> GetGlobalNavigations(string clientId, Guid? userId = null);

    Task<List<ProjectModel>> GetNavigationsByAppId(params string[] appIds);

    Task<NavDetailModel?> GetMenuDetailAsync(Guid menuId);

    Task UpdateMenuAsync(UpdateNavModel input);

    Task<List<ProjectModel>> GetUIAndMenusAsync();
}
