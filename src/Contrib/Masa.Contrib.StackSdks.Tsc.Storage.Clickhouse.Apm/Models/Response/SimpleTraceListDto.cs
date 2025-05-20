// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Tsc.Storage.Clickhouse.Apm.Models.Response;

public class SimpleTraceListDto
{
    public string TraceId { get; set; } = default!;

    public DateTime Timestamp { get; set; }

    public DateTime EndTimestamp { get; set; }
}
