namespace Masa.BuildingBlocks.StackSdks.Tsc.Service;

public interface ITraceService
{
    Task<IEnumerable<TraceResponseDto>> GetAsync(string traceId, DateTime start, DateTime end);

    Task<IEnumerable<TraceResponseDto>> GetBatchAsync(IEnumerable<string> traceIds, DateTime start, DateTime end);

    Task<int[]> GetErrorStatusAsync();
}