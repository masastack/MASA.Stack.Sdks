// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Pm.Model;

public class ProjectDetailModel : ModelBase
{
    public int Id { get; set; }

    public string Identity { get; set; } = "";

    public string LabelCode { get; set; } = default!;

    public string Name { get; set; } = "";

    public string Description { get; set; } = "";

    public List<EnvironmentProjectTeamDto> EnvironmentProjectTeams { get; set; } = new();

    public List<int> EnvironmentClusterIds { get; set; } = new List<int>();
}
