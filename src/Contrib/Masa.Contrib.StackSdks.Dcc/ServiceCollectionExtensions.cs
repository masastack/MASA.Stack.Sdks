// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDccClient(this IServiceCollection services, string sectionName = "DccOptions", Action<IConnectionMultiplexer>? connectConfig = null)
    {
        services.AddConfigure<RedisConfigurationOptions>($"{sectionName}:RedisOptions", DEFAULT_CLIENT_NAME);
        return services.AddDccClientCore(connectConfig: connectConfig);
    }

    public static IServiceCollection AddDccClient(this IServiceCollection services, Action<RedisConfigurationOptions> options, Action<IConnectionMultiplexer>? connectConfig = null)
    {
        services.Configure(DEFAULT_CLIENT_NAME, options);
        return services.AddDccClientCore(connectConfig: connectConfig);
    }

    public static IServiceCollection AddDccClient(this IServiceCollection services, RedisConfigurationOptions options, Action<IConnectionMultiplexer>? connectConfig = null)
    {
        services.AddDistributedCache(DEFAULT_CLIENT_NAME, distributedCacheOptions =>
        {
            distributedCacheOptions.UseStackExchangeRedisCache(options, connectConfig: connectConfig);
        });

        return services.AddDccClientCore(false, connectConfig: connectConfig);
    }

    private static IServiceCollection AddDccClientCore(this IServiceCollection services, bool isUseStackExchangeRedisCache = true, Action<IConnectionMultiplexer>? connectConfig = null)
    {
        if (isUseStackExchangeRedisCache)
            services.AddDistributedCache(DEFAULT_CLIENT_NAME, distributedCacheOptions =>
            {
                distributedCacheOptions.UseStackExchangeRedisCache(connectConfig: connectConfig);
            });

        services.AddScoped<IDccClient>(serviceProvider =>
        {
            return new DccClient(serviceProvider.GetRequiredService<IDistributedCacheClientFactory>().Create(DEFAULT_CLIENT_NAME), serviceProvider.GetRequiredService<ICallerFactory>());
        });
        MasaApp.TrySetServiceCollection(services);
        return services;
    }
}
