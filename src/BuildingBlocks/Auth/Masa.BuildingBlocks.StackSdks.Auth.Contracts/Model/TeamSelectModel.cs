// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class TeamSelectModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Avatar { get; set; } = string.Empty;
}
