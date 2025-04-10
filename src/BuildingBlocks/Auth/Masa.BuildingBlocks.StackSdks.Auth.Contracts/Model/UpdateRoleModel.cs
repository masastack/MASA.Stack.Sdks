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

    public UpdateRoleModel(Guid id, string name, string code, string? description, bool enabled, int limit, RoleTypes type, List<SubjectPermissionRelationModel> permissions, List<Guid> childRoles, List<string> clients)
    {
        Id = id;
        Name = name;
        Code = code;
        Description = description;
        Enabled = enabled;
        Permissions = permissions;
        ChildrenRoles = childRoles;
        Limit = limit;
        Type = type;
        Clients = clients;
    }

    public static implicit operator UpdateRoleModel(RoleDetailModel role)
    {
        return new UpdateRoleModel(role.Id, role.Name, role.Code, role.Description, role.Enabled, role.Limit, role.Type, role.Permissions, role.ChildrenRoles, role.Clients);
    }
}
