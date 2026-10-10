// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    private static async Task InitializeMasaStackConfiguration(
        IServiceCollection services,
        Dictionary<string, string> configs)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configurationApiManage = serviceProvider.GetRequiredService<IConfigurationApiManage>();
        var configurationApiClient = serviceProvider.GetRequiredService<IConfigurationApiClient>();

        try
        {
            var remoteConfigs = await configurationApiClient.GetAsync<Dictionary<string, string>>(
                configs[MasaStackConfigConstant.ENVIRONMENT],
                configs[MasaStackConfigConstant.CLUSTER],
                DEFAULT_PUBLIC_ID,
                DEFAULT_CONFIG_NAME);

            if (remoteConfigs != null)
            {
                await configurationApiManage.UpdateAsync(
                    configs[MasaStackConfigConstant.ENVIRONMENT],
                    configs[MasaStackConfigConstant.CLUSTER],
                    DEFAULT_PUBLIC_ID,
                    DEFAULT_CONFIG_NAME,
                    configs);
            }
        }
        catch
        {
            // remoteConfigs is null
            await configurationApiManage.AddAsync(
                configs[MasaStackConfigConstant.ENVIRONMENT],
                configs[MasaStackConfigConstant.CLUSTER],
                DEFAULT_PUBLIC_ID,
                new Dictionary<string, object>
                {
                    { DEFAULT_CONFIG_NAME, configs }
                });
        }
    }

    public static async Task<IServiceCollection> AddMasaStackConfigAsync(this IServiceCollection services, MasaStackProject project, MasaStackApp app,
        bool init = false,
        DccOptions? dccOptions = null,
        Action<IMasaCallerClientBuilder>? callerAction = null)
    {
        var configs = GetConfigMap(services);

        dccOptions ??= MasaStackConfigUtils.GetDefaultDccOptions(configs, project, app);
        services.AddSingleton(dccOptions);
        services.AddMasaConfiguration(builder => builder.UseDcc(dccOptions, action: (CallerBuilder callerBuilder) =>
        {
            callerBuilder.UseDccHttpClient(dccOptions.ManageServiceAddress, callerAction);
        }));

        if (init)
        {
            await InitializeMasaStackConfiguration(services, configs).ConfigureAwait(false);
        }

        RegisterMasaStackConfig(services, configs);
        return services;
    }

    public static async Task<IServiceCollection> AddMasaStackConfigDaprAsync(this IServiceCollection services, MasaStackProject project, MasaStackApp app,
        bool init = false,
        DccDaprOptions? dccOptions = null,
        Action<IMasaCallerClientBuilder>? callerAction = null)
    {
        var configs = GetConfigMap(services);

        dccOptions ??= MasaStackConfigUtils.GetDefaultDccDaprOptions(configs, project, app);
        services.AddSingleton(dccOptions);
        services.AddMasaConfiguration(builder => builder.UseDcc(dccOptions, action: (CallerBuilder callerBuilder) =>
        {
            callerBuilder.UseDccHttpClient(dccOptions.ManageServiceAddress, callerAction);
        }));

        if (init)
        {
            await InitializeMasaStackConfiguration(services, configs).ConfigureAwait(false);
        }

        RegisterMasaStackConfig(services, configs);
        return services;
    }

    public static DccDaprOptions? GetDccDaprOptions(this IServiceCollection services)
    {
        return services.BuildServiceProvider().GetService<DccDaprOptions>();
    }

    public static IMasaStackConfig GetMasaStackConfig(this IServiceCollection services)
    {
        return services.BuildServiceProvider().GetRequiredService<IMasaStackConfig>();
    }

    public static DccOptions? GetDccOptions(this IServiceCollection services)
    {
        return services.BuildServiceProvider().GetService<DccOptions>();
    }

    public static IMultiEnvironmentMasaStackConfig GetMultiEnvironmentMasaStackConfig(this IServiceCollection services)
    {
        return services.BuildServiceProvider().GetRequiredService<IMultiEnvironmentMasaStackConfig>();
    }

    private static void RegisterMasaStackConfig(IServiceCollection services, Dictionary<string, string> configs)
    {
        services.TryAddScoped<IMasaStackConfig>(serviceProvider =>
        {
            var configurationApiClient = serviceProvider.GetRequiredService<IConfigurationApiClient>();
            return new MasaStackConfig(configurationApiClient, configs);
        });

        services.TryAddScoped<IMultiEnvironmentMasaStackConfig>(serviceProvider =>
        {
            var configurationApiClient = serviceProvider.GetRequiredService<IConfigurationApiClient>();
            return new MultiEnvironmentMasaStackConfig(configurationApiClient, configs);
        });
    }

    private static Dictionary<string, string> GetConfigMap(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        string environment = configuration.GetValue<string>(MasaStackConfigConstant.ENVIRONMENT) ?? string.Empty;
        if (string.IsNullOrWhiteSpace(environment))
            environment = configuration["ASPNETCORE_ENVIRONMENT"] ?? string.Empty;

        // Missing local keys are empty; Dapr path fills them from $public.DefaultConfig via MasaStackConfig.GetValues().
        string Get(string key) => configuration.GetValue<string>(key) ?? string.Empty;

        var configs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { MasaStackConfigConstant.VERSION, Get(MasaStackConfigConstant.VERSION) },
            { MasaStackConfigConstant.IS_DEMO, configuration.GetValue<bool>(MasaStackConfigConstant.IS_DEMO).ToString() },
            { MasaStackConfigConstant.DOMAIN_NAME, Get(MasaStackConfigConstant.DOMAIN_NAME) },
            { MasaStackConfigConstant.NAMESPACE, Get(MasaStackConfigConstant.NAMESPACE) },
            { MasaStackConfigConstant.CLUSTER, Get(MasaStackConfigConstant.CLUSTER) },
            { MasaStackConfigConstant.OTLP_URL, Get(MasaStackConfigConstant.OTLP_URL) },
            { MasaStackConfigConstant.REDIS, Get(MasaStackConfigConstant.REDIS) },
            { MasaStackConfigConstant.CONNECTIONSTRING, Get(MasaStackConfigConstant.CONNECTIONSTRING) },
            { MasaStackConfigConstant.MASA_STACK, Get(MasaStackConfigConstant.MASA_STACK) },
            { MasaStackConfigConstant.ELASTIC, Get(MasaStackConfigConstant.ELASTIC) },
            { MasaStackConfigConstant.ENVIRONMENT, environment },
            { MasaStackConfigConstant.ADMIN_PWD, Get(MasaStackConfigConstant.ADMIN_PWD) },
            { MasaStackConfigConstant.DCC_SECRET, Get(MasaStackConfigConstant.DCC_SECRET) },
            { MasaStackConfigConstant.SUFFIX_IDENTITY, Get(MasaStackConfigConstant.SUFFIX_IDENTITY) },
            { MasaStackConfigConstant.DCC_STORE_NAME, Get(MasaStackConfigConstant.DCC_STORE_NAME) }
        };
        return configs;
    }

    private static IMasaCallerClientBuilder UseDccHttpClient(this CallerBuilder callerBuilder, string dccHost, Action<IMasaCallerClientBuilder>? callerAction = null)
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
        var builder = callerBuilder.UseHttpClient(builder =>
        {
            builder.BaseAddress = dccHost;
            builder.Configure = http =>
            {
                http.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", $"masastack_sdk/{version} (dcc; http)");
            };
        });
        callerAction?.Invoke(builder);
        return builder;
    }
}
