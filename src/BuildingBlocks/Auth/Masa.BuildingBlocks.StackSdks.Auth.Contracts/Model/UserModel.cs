// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class UserModel : UserListModel
{
    public List<RoleModel> Roles { get; set; } = new();

    public List<SubjectPermissionRelationModel> Permissions { get; set; } = new();

    public Dictionary<string, string> ClaimData { get; set; } = new();

}
