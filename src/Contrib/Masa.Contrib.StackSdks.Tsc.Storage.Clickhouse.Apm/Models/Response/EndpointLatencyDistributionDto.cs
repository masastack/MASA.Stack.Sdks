// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Tsc.Storage.Clickhouse.Apm.Models.Response;

public class EndpointLatencyDistributionDto
{
    public long? P95 { get; set; }

    public IEnumerable<ChartPointDto> Latencies { get; set; } = default!;
}
