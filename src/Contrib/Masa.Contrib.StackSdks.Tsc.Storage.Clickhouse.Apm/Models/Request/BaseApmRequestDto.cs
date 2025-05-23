// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Tsc.Storage.Clickhouse.Apm.Models.Request;

public class BaseApmRequestDto : RequestPageBase
{
    public string? Env { get; set; }

    public ComparisonTypes? ComparisonType { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public string Period { get; set; } = default!;

    public string? Service { get; set; }

    public string? Queries { get; set; }

    public string? OrderField { get; set; }

    public bool? IsDesc { get; set; }

    public string StatusCodes { get; set; } = default!;

    public bool HasPage { get; set; } = true;

    public string? ExType { get; set; }

    public string? TraceId { get; set; }

    public string? TextField { get; set; }

    public string? TextValue { get; set; }

    public string? ExMessage { get; set; } 

    [JsonIgnore]
    public List<string> AppIds { get; set; } = default!;
}
