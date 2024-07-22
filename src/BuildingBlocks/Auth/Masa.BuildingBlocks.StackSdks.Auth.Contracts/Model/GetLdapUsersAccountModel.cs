namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class GetLdapUsersAccountModel
{
    public List<Guid> UserIds { get; set; } = new();
}
