namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class ThirdPartyIdpSelectModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;

    public AuthenticationTypes AuthenticationType { get; set; }
}
