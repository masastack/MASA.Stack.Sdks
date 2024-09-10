// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Config;

public class MultiEnvironmentMasaStackConfig : MasaStackConfig, IMultiEnvironmentMasaStackConfig
{
    public MultiEnvironmentMasaStackConfig(IClientScopeServiceProviderAccessor clientScopeServiceProviderAccessor, Dictionary<string, string> configs) : base(clientScopeServiceProviderAccessor, configs)
    {

    }

    public IMasaStackConfig SetEnvironment(string environment)
    {
        var configs = GetValues();
        configs[MasaStackConfigConstant.ENVIRONMENT] = environment;

        return this;
    }
}