// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Tsc.Storage.Clickhouse.Apm.Models.Response;

public class EndpointChartDto
{
    public IEnumerable<ChartPointDto> P99s { get; set; } = default!;

    public IEnumerable<ChartPointDto> P95s { get; set; } = default!;

    public IEnumerable<ChartPointDto> Latencies { get; set; } = default!;

    public IEnumerable<ChartPointDto> Throughputs { get; set; } = default!;

    public IEnumerable<ChartPointDto> Fails { get; set; } = default!;
}

public class ErrorMessageDto
{
    public string Type { get; set; } = default!;

    public string Message { get; set; } = default!;

    public DateTime LastTime { get; set; }

    public int Total { get; set; }
}
