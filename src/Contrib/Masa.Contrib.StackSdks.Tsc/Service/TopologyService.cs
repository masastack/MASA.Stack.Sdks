namespace Masa.Contrib.StackSdks.Tsc.Service;

internal class TopologyService : ITopologyService
{
    readonly ICaller _caller;

    public TopologyService(ICaller caller)
    {
        _caller = caller;
    }

    public Task<IEnumerable<ServiceTopologiesDto>> GetServicesAsync(string? service = null)
    {
        return _caller.GetAsync<IEnumerable<ServiceTopologiesDto>>("api/topology/services", new { service })!;
    }
}
