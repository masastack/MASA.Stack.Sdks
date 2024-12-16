// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static async Task<IServiceCollection> AddMasaStackConfigAsync(this IServiceCollection services, IConfiguration configuration, string environment)
    {
        var configs = GetConfigMap(configuration, environment);
    
        services.TryAddScoped<MasaComponentsClaimsCache>();
        services.TryAddSingleton<IClientScopeServiceProviderAccessor, ComponentsClientScopeServiceProviderAccessor>();
        
        services.TryAddScoped<IMasaStackConfig>(sp =>
        {
            var clientScopeServiceProviderAccessor = sp.GetRequiredService<IClientScopeServiceProviderAccessor>();
            return new MasaStackConfig(clientScopeServiceProviderAccessor, configs);
        });

        services.TryAddScoped<IMultiEnvironmentMasaStackConfig>(sp =>
        {
            var clientScopeServiceProviderAccessor = sp.GetRequiredService<IClientScopeServiceProviderAccessor>();
            return new MultiEnvironmentMasaStackConfig(clientScopeServiceProviderAccessor, configs);
        });
        return services;
    }

    public static IMasaStackConfig GetMasaStackConfig(this IServiceCollection services)
    {
        return services.BuildServiceProvider().GetRequiredService<IMasaStackConfig>();
    }

    public static IMultiEnvironmentMasaStackConfig GetMultiEnvironmentMasaStackConfig(this IServiceCollection services)
    {
        return services.BuildServiceProvider().GetRequiredService<IMultiEnvironmentMasaStackConfig>();
    }

    private static Dictionary<string, string> GetConfigMap(IConfiguration configuration, string environment)
    {
        var configs = new Dictionary<string, string>()
        {
            { MasaStackConfigConstant.VERSION, configuration.GetValue<string>(MasaStackConfigConstant.VERSION) },
            { MasaStackConfigConstant.IS_DEMO, configuration.GetValue<bool>(MasaStackConfigConstant.IS_DEMO).ToString() },
            { MasaStackConfigConstant.DOMAIN_NAME, configuration.GetValue<string>(MasaStackConfigConstant.DOMAIN_NAME) },
            { MasaStackConfigConstant.NAMESPACE, configuration.GetValue<string>(MasaStackConfigConstant.NAMESPACE) },
            { MasaStackConfigConstant.CLUSTER, configuration.GetValue<string>(MasaStackConfigConstant.CLUSTER) },
            { MasaStackConfigConstant.OTLP_URL, configuration.GetValue<string>(MasaStackConfigConstant.OTLP_URL) },
            { MasaStackConfigConstant.MASA_STACK, configuration.GetValue<string>(MasaStackConfigConstant.MASA_STACK) },
            { MasaStackConfigConstant.ENVIRONMENT, environment }
        };
        return configs;
    }
}
