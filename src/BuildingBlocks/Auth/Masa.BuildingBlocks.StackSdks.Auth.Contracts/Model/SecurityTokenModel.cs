namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class SecurityTokenModel
{
    public string Region { get; set; }

    public string AccessKeyId { get; set; }

    public string AccessKeySecret { get; set; }

    public string StsToken { get; set; }

    public string Bucket { get; set; }
}
