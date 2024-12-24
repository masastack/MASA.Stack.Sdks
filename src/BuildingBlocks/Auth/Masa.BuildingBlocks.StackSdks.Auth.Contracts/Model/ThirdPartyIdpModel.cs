// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class ThirdPartyIdpModel
{
    public string ClientId { get; set; } = default!;

    public string ClientSecret { get; set; } = default!;

    public string Url { get; set; } = default!;

    public AuthenticationTypes VerifyType { get; set; }

    public string Name { get; set; } = default!;

    public string DisplayName { get; set; } = default!;

    public string Icon { get; set; } = default!;

    public ThirdPartyIdpTypes ThirdPartyIdpType { get; set; }

    public string CallbackPath { get; set; } = "";

    public string AuthorizationEndpoint { get; set; } = "";

    public string TokenEndpoint { get; set; } = "";

    public string UserInformationEndpoint { get; set; } = "";

    public bool MapAll { get; set; }

    public Dictionary<string, string> JsonKeyMap { get; set; } = new();
}
