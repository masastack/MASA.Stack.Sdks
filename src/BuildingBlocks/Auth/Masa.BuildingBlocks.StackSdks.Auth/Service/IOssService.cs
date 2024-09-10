namespace Masa.BuildingBlocks.StackSdks.Auth.Service;

public interface IOssService
{
    Task<SecurityTokenModel> GetSecurityTokenAsync();
}
