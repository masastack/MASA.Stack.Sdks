// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Tsc.Contracts.Model.Trace;

public class TraceRequestAttrDto
{
    public string Service { get; set; } = default!;

    public string Instance { get; set; } = default!;

    public string Endpoint { get; set; } = default!;

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public string Query { get; set; } = default!;

    public int MaxCount { get; set; }
}
