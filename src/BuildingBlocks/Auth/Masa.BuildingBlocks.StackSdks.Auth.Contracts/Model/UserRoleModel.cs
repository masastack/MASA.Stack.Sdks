namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class UserRoleModel
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public DateTime BindTime { get; set; }
}
