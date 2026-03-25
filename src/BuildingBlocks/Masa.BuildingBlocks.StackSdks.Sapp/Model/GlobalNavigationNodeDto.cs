// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Sapp.Model;

public class GlobalNavigationNodeDto
{
    public Guid? Id { get; set; }

    public Guid PermissionId { get; set; }

    public string Code { get; set; } = string.Empty;

    public GlobalNavigationTypes NavigationType { get; set; }

    public GlobalNavigationOpenTypes OpenType { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;

    public bool IsHidden { get; set; }

    public List<GlobalNavigationNodeDto> Children { get; set; } = new();
}
