namespace Masa.BuildingBlocks.StackSdks.Dcc.Contracts.Model;

public class OssSecurityTokenDto
{
    public string Region { get; set; }

    public string AccessKeyId { get; set; }

    public string AccessKeySecret { get; set; }

    public string StsToken { get; set; }

    public string Bucket { get; set; }

    public string CdnHost { get; set; }

    public OssSecurityTokenDto(string region, string accessKeyId, string accessKeySecret, string stsToken, string bucket, string cdnHost)
    {
        Region = region;
        AccessKeyId = accessKeyId;
        AccessKeySecret = accessKeySecret;
        StsToken = stsToken;
        Bucket = bucket;
        CdnHost = cdnHost;
    }
}
