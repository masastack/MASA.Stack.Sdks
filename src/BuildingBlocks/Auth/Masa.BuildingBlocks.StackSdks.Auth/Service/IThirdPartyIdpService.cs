// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Auth.Service;

public interface IThirdPartyIdpService
{
    Task<List<ThirdPartyIdpModel>> GetAllAsync();

    Task<List<ThirdPartyIdpModel>> GetAllFromCacheAsync();

    Task<LdapOptionsModel?> GetLdapOptionsAsync(string scheme);

    Task<List<ThirdPartyIdpSelectModel>> GetSelectAsync(string? search, bool includeLdap);
}
