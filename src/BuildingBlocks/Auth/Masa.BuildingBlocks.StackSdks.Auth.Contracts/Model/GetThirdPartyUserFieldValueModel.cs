namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class GetThirdPartyUserFieldValueModel
{
    public string Scheme { get; set; } = default!;

    public string? Field { get; set; }

    public List<Guid> UserIds { get; set; } = new();
}
