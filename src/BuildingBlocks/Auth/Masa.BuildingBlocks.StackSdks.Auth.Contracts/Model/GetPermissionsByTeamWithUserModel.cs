// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class GetPermissionsByTeamWithUserModel
{
    public Guid User { get; set; }

    public List<Guid> Teams { get; set; } = new();
}
