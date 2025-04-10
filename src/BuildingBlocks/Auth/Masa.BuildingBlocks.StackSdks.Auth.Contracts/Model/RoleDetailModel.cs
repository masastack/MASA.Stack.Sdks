// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class RoleDetailModel: RoleModel
{
    public List<SubjectPermissionRelationModel> Permissions { get; set; } = new();

    public List<Guid> ParentRoles { get; set; } = new();

    public List<Guid> ChildrenRoles { get; set; } = new();

    public List<UserSelectModel> Users { get; set; } = new();

    public List<Guid> Teams { get; set; } = new();

    public int AvailableQuantity { get; set; }

    public List<string> Clients { get; set; } = new();
}
