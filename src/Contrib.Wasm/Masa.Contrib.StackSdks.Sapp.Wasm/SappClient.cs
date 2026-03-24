// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Sapp;

public class SappClient : ISappClient
{
    public SappClient(ICaller caller)
    {
        GlobalNavService = new GlobalNavService(caller);
    }

    public IGlobalNavService GlobalNavService { get; init; }
}
