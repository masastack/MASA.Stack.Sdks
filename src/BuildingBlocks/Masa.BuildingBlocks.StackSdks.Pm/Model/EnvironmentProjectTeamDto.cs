namespace Masa.BuildingBlocks.StackSdks.Pm.Model;

public class EnvironmentProjectTeamDto
{
    public string EnvironmentName { get; set; } = "";

    public List<Guid> TeamIds { get; set; } = new();
}
