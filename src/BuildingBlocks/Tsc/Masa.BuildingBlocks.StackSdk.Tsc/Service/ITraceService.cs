namespace Masa.BuildingBlocks.StackSdks.Tsc.Service;

public interface ITraceService
{
    Task<IEnumerable<TraceResponseDto>> GetAsync(string traceId, DateTime start, DateTime end);

    Task<int[]> GetErrorStatusAsync();
}