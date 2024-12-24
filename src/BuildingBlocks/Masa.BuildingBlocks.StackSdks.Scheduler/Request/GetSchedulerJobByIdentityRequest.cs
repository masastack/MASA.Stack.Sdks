// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Scheduler.Request;

public class GetSchedulerJobByIdentityRequest
{
    public string JobIdentity { get; set; } = default!;

    public string ProjectIdentity { get; set; } = default!;
}
