// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class UpdateRoleModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public string? Description { get; set; }

    public bool Enabled { get; set; }

    public int Limit { get; set; }

    public List<SubjectPermissionRelationModel> Permissions { get; set; } = new();

    public List<Guid> ChildrenRoles { get; set; } = new();

    public RoleTypes Type { get; set; }

    public List<string> Clients { get; set; } = new();
}
