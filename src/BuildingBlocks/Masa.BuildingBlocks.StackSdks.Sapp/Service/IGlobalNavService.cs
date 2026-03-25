// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Sapp.Service;

public interface IGlobalNavService
{
    Task<List<GlobalNavigationAppDto>> GetGlobalNavigationsByClientIdAsync(string clientId);

    Task<List<GlobalNavigationNodeDto>> GetMenusByPmIdentityAsync(string pmIdentity);

    Task<List<AppEntryDto>> GetVisibleAppEntriesByClientIdAsync(string clientId);
}
