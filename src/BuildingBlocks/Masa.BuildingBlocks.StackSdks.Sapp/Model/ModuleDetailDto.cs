namespace Masa.BuildingBlocks.StackSdks.Sapp.Model;

public class ModuleDetailDto
{
    public string Code { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string? Icon { get; set; }

    public string PmIdentity { get; set; } = default!;

    public bool Enable { get; set; } = default!;

    public bool Show { get; set; } = default!;

    public StatusTypes Status { get; set; }

    public MaintainceDto? Maintaince { get; set; }
}