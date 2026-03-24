// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSappClient(this IServiceCollection services, string sappServiceBaseAddress)
    {
        MasaArgumentException.ThrowIfNullOrEmpty(sappServiceBaseAddress);

        return services.AddSappClient(callerBuilder =>
        {
            callerBuilder.UseHttpClient(builder =>
            {
                builder.BaseAddress = sappServiceBaseAddress;
            }).UseAuthentication();
        });
    }

    public static IServiceCollection AddSappClient(this IServiceCollection services, Func<string> sappServiceBaseAddressFunc)
    {
        MasaArgumentException.ThrowIfNull(sappServiceBaseAddressFunc);

        return services.AddSappClient(callerBuilder =>
        {
            callerBuilder.UseHttpClient(builder =>
            {
                builder.BaseAddress = sappServiceBaseAddressFunc.Invoke();
            }).UseAuthentication();
        });
    }

    public static IServiceCollection AddSappClient(this IServiceCollection services, Action<CallerBuilder> callerBuilder)
    {
        MasaArgumentException.ThrowIfNull(callerBuilder);

        if (services.Any(service => service.ServiceType == typeof(ISappClient)))
            return services;

        services.AddCaller(DEFAULT_CLIENT_NAME, callerBuilder.Invoke);

        services.AddScoped<ISappClient>(serviceProvider =>
        {
            var callProvider = serviceProvider.GetRequiredService<ICallerFactory>().Create(DEFAULT_CLIENT_NAME);
            var sappClient = new SappClient(callProvider);
            return sappClient;
        });

        MasaApp.TrySetServiceCollection(services);
        return services;
    }
}
