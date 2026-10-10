namespace Masa.BuildingBlocks.StackSdks.Tsc.Model;

public class ServiceTopologiesDto
{
    public string Service { get; set; } = default!;

    public IEnumerable<string> Servers { get; set; } = default!;
}
