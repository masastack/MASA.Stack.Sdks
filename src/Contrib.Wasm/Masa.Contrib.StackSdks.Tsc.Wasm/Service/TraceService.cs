namespace Masa.Contrib.StackSdks.Tsc.Service;

internal class TraceService : ITraceService
{
    readonly ICaller Caller;

    const string RootPath = "api/trace";

    public TraceService(ICaller caller)
    {
        Caller = caller;
    }

    public async Task<IEnumerable<TraceResponseDto>> GetAsync(string traceId, DateTime start, DateTime end)
    {
        return await Caller.GetAsync<IEnumerable<TraceResponseDto>>($"{RootPath}/{traceId}?start={start}&end={end}") ?? Array.Empty<TraceResponseDto>();
    }

    public async Task<IEnumerable<TraceResponseDto>> GetBatchAsync(IEnumerable<string> traceIds, DateTime start, DateTime end)
    {
        var traceIdList = string.Join(",", traceIds);
        return await Caller.GetAsync<IEnumerable<TraceResponseDto>>($"{RootPath}/batch?traceIds={traceIdList}&start={start}&end={end}") ?? Array.Empty<TraceResponseDto>();
    }

    public async Task<int[]> GetErrorStatusAsync()
    {
        return (await Caller.GetAsync<int[]>($"{RootPath}/errorStatus"))!;
    }
}
