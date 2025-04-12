// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Auth.Service;

public interface IUserClaimService
{
    Task<List<UserClaimSelectModel>> GetSelectAsync(string? search);
}
