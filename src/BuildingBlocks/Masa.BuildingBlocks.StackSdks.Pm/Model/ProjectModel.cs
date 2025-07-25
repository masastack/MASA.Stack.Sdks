// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Pm.Model;

public class ProjectModel
{
    public int Id { get; set; }

    public string Identity { get; set; } = "";

    public string Name { get; set; } = "";

    public List<EnvironmentProjectTeamDto> EnvironmentProjectTeams { get; set; } = new();

    public string Description { get; set; } = "";

    public string LabelCode { get; set; } = "";

    public string LabelName { get; set; } = "";

    public Guid Modifier { get; set; }

    public string? ModifierName { get; set; }

    public DateTime ModificationTime { get; set; }
}
