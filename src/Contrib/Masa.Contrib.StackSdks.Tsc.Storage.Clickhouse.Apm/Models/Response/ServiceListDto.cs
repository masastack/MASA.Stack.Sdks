// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Tsc.Storage.Clickhouse.Apm.Models.Response;

public class ServiceListDto
{
    public string Service { get; set; } = default!;

    public string Name { get; set; } = default!;

    public IEnumerable<string> Envs { get; set; } = default!;

    public long Latency { get; set; }

    public double Throughput { get; set; }

    public double Failed { get; set; }
}

public class ChartLineDto
{
    public string Name { get; set; } = default!;

    public IEnumerable<ChartLineItemDto> Currents { get; set; } = default!;

    public IEnumerable<ChartLineItemDto> Previous { get; set; } = default!;
}

public class ChartLineCountDto
{
    public string Name { get; set; } = default!;

    public IEnumerable<ChartLineCountItemDto> Currents { get; set; } = default!;

    public IEnumerable<ChartLineCountItemDto> Previous { get; set; } = default!;
}

public class ChartLineCountItemDto
{
    public long Time { get; set; }

    public object Value { get; set; } = default!;
}

public class ChartLineItemDto
{
    public long Time { get; set; }

    public long Latency { get; set; }

    public double P99 { get; set; }

    public double P95 { get; set; }

    public double Throughput { get; set; }

    public double Failed { get; set; }
}

public class ChartPointDto
{
    public string X { get; set; } = default!;

    public string Y { get; set; } = default!;
}
