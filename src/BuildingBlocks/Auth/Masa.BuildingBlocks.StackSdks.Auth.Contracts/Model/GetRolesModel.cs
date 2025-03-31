// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class GetRolesModel: PaginatedOptions
{
    public string Search { get; set; }

    public bool? Enabled { get; set; }

    public string ClientId { get; set; }
}
