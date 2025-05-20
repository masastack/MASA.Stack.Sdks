// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Tsc.Storage.Clickhouse.Apm.Models.Request;

public class ApmErrorRequestDto : ApmEndpointRequestDto
{
    public bool Filter { get; set; } = true;
}
