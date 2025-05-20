namespace Masa.Contrib.StackSdks.Tsc.Service;

internal class ApmService : IApmService
{
    readonly ICaller Caller;
    const string RootPath = "/api/apm";

    public ApmService(ICaller caller)
    {
        Caller = caller;
    }

    public Task<PaginatedListBase<ErrorMessageDto>> GetErrorsPageAsync(Guid teamId, ApmErrorRequestDto query, string? projectId = default, string? appType = default, bool ignoreTeam = false) => Caller.GetAsync<PaginatedListBase<ErrorMessageDto>>($"{RootPath}/errors?teamId={teamId}&project={projectId}&ignoreTeam={ignoreTeam}{(string.IsNullOrEmpty(appType) ? "" : $"&appType={appType}")}", data: query)!;

    public Task<List<ChartPointDto>> GetSpanErrorsAsync(ApmEndpointRequestDto query) => Caller.GetAsync<List<ChartPointDto>>($"{RootPath}/spanErrors", data: query)!;

    public Task<List<ChartLineDto>> GetChartsAsync(ApmEndpointRequestDto query) => Caller.GetAsync<List<ChartLineDto>>($"{RootPath}/charts", data: query)!;

    public Task<List<ChartLineCountDto>> GetErrorChartAsync(ApmEndpointRequestDto query) => Caller.GetAsync<List<ChartLineCountDto>>($"{RootPath}/errorChart", data: query)!;

    public Task<List<ChartLineCountDto>> GetLogChartAsync(ApmEndpointRequestDto query) => Caller.GetAsync<List<ChartLineCountDto>>($"{RootPath}/logChart", data: query)!;

    public Task<EndpointLatencyDistributionDto> GetLatencyDistributionAsync(ApmEndpointRequestDto query) => Caller.GetAsync<EndpointLatencyDistributionDto>($"{RootPath}/latencyDistributions", data: query)!;

    public Task<Dictionary<string, List<EnvironmentAppDto>>> GetEnvironmentServiceAsync(Guid teamId, DateTime start, DateTime end, string? env = default, bool ignoreTeam = false) => Caller.GetAsync<Dictionary<string, List<EnvironmentAppDto>>>($"{RootPath}/EnvironmentService", data: new { teamId, start, end, env, ignoreTeam })!;

    public Task<PaginatedListBase<LogResponseDto>> GetLogListAsync(Guid teamId, BaseRequestDto query, string? projectId = default, string? appType = default, bool ignoreTeam = false) => Caller.PostAsync<PaginatedListBase<LogResponseDto>>($"{RootPath}/logList?teamId={teamId}&project={projectId}&&ignoreTeam={ignoreTeam}{(string.IsNullOrEmpty(appType) ? "" : $"&appType={appType}")}", query)!;

    public Task<List<string>> GetStatusCodesAsync() => Caller.GetAsync<List<string>>($"{RootPath}/statuscodes")!;

    public Task<List<string>> GetEndpointsAsync(BaseRequestDto query) => Caller.PostAsync<List<string>>($"{RootPath}/endpointList", query)!;

    public Task<List<string>> GetExceptionTypesAsync(BaseRequestDto query) => Caller.PostAsync<List<string>>($"{RootPath}/errorTypes", query)!;

    public Task<PaginatedListBase<SimpleTraceListDto>> GetSimpleTraceListAsync(ApmTraceLatencyRequestDto query) => Caller.PostAsync<PaginatedListBase<SimpleTraceListDto>>($"{RootPath}/simpleTraceList", query)!;
}
