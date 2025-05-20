namespace Masa.BuildingBlocks.StackSdks.Tsc.Service;

public interface IApmService
{
    Task<PaginatedListBase<ErrorMessageDto>> GetErrorsPageAsync(Guid teamId, ApmErrorRequestDto query, string? projectId = default, string? appType = default, bool ignoreTeam = false);

    Task<List<ChartPointDto>> GetSpanErrorsAsync(ApmEndpointRequestDto query);

    Task<List<ChartLineDto>> GetChartsAsync(ApmEndpointRequestDto query);

    Task<List<ChartLineCountDto>> GetErrorChartAsync(ApmEndpointRequestDto query);

    Task<List<ChartLineCountDto>> GetLogChartAsync(ApmEndpointRequestDto query) ;

    Task<EndpointLatencyDistributionDto> GetLatencyDistributionAsync(ApmEndpointRequestDto query) ;

    Task<Dictionary<string, List<EnvironmentAppDto>>> GetEnvironmentServiceAsync(Guid teamId, DateTime start, DateTime end, string? env = default, bool ignoreTeam = false);

    Task<PaginatedListBase<LogResponseDto>> GetLogListAsync(Guid teamId, BaseRequestDto query, string? projectId = default, string? appType = default, bool ignoreTeam = false);

    Task<List<string>> GetStatusCodesAsync() ;

    Task<List<string>> GetEndpointsAsync(BaseRequestDto query);

    Task<List<string>> GetExceptionTypesAsync(BaseRequestDto query) ;

    Task<PaginatedListBase<SimpleTraceListDto>> GetSimpleTraceListAsync(ApmTraceLatencyRequestDto query) ;
}
