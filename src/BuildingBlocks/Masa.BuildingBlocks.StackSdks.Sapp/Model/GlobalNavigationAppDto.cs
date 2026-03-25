// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Sapp.Model;

public class GlobalNavigationAppDto
{
    public Guid Id { get; set; }

    public string Identity { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Tag { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;

    public StatusTypes Status { get; set; }

    public List<GlobalNavigationNodeDto> Navs { get; set; } = new();
}
