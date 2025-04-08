// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class ClientSelectModel
{
    public Guid Id { get; set; }

    public string ClientName { get; set; } = string.Empty;

    public string LogoUri { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;

    public ClientTypes ClientType { get; set; }
}
