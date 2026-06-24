namespace Masa.BuildingBlocks.StackSdks.Sapp.Model;

public class MaintainceDto
{
    public string Reason { get; set; } = default!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
}
