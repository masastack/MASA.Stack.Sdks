// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class GetDynamicRoleInputModel
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;

    public string Search { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;
}
