namespace Masa.BuildingBlocks.StackSdks.Tsc.Service;

public interface ITopologyService
{
    public Task<IEnumerable<ServiceTopologiesDto>> GetServicesAsync(string? service = default);
}