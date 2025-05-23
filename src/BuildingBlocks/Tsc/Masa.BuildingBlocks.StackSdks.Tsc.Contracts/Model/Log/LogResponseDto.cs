// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Tsc.Contracts.Log;

public class LogResponseDto
{
    public virtual DateTime Timestamp { get; set; }

    public virtual string TraceId { get; set; } = default!;

    public virtual string SpanId { get; set; } = default!;

    public virtual int TraceFlags { get; set; }

    public virtual string SeverityText { get; set; } = default!;

    public virtual int SeverityNumber { get; set; }

    public virtual object Body { get; set; } = default!;

    public virtual Dictionary<string, object> Resource { get; set; } = default!;

    public virtual Dictionary<string, object> Attributes { get; set; } = default!;
}
