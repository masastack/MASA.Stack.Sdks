﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class RoleSelectModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public int Limit { get; set; }

    public int AvailableQuantity { get; set; }
}
