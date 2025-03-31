// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class RoleModel
{
    public Guid Id { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public int Limit { get; set; }

    public RoleTypes Type { get; set; }

    public string? Description { get; set; }

    public bool Enabled { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? ModificationTime { get; set; }

    public string? Creator { get; set; }

    public string? Modifier { get; set; }
}
