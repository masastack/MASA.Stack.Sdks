namespace Masa.Contrib.StackSdks.Tsc.Service;

internal class TraceService : ITraceService
{
    readonly ICaller Caller;

    public TraceService(ICaller caller)
    {
        Caller = caller;
    }

    public async Task<IEnumerable<TraceResponseDto>> GetAsync(string traceId, DateTime start, DateTime end)
    {
        return await Caller.GetAsync<IEnumerable<TraceResponseDto>>($"/api/trace/{traceId}?start={start}&end={end}") ?? Array.Empty<TraceResponseDto>();
    }
}
