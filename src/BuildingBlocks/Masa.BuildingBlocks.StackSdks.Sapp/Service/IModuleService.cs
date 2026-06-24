namespace Masa.BuildingBlocks.StackSdks.Sapp.Service;

public interface IModuleService
{
    Task<ModuleDetailDto?> GetByPmIdentityAsync(string pmIdentity);
}