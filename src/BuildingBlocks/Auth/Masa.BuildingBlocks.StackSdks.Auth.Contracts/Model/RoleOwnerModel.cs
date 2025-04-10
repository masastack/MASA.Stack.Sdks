// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class RoleOwnerModel
{
    public List<UserSelectModel> Users { get; set; } = new();

    public List<TeamSelectModel> Teams { get; set; } = new();
}
