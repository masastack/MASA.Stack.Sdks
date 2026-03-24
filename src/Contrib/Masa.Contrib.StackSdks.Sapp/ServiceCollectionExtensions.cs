// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSappClient(this IServiceCollection services, string sappServiceBaseAddress, Action<IMasaCallerClientBuilder>? callerAction = default)
    {
        return services.AddSappClient(() => sappServiceBaseAddress, callerAction);
    }

    public static IServiceCollection AddSappClient(this IServiceCollection services, Func<string> sappServiceBaseAddressFunc, Action<IMasaCallerClientBuilder>? callerAction = default)
    {
        MasaArgumentException.ThrowIfNull(sappServiceBaseAddressFunc);
        var url = sappServiceBaseAddressFunc.Invoke();
        MasaArgumentException.ThrowIfNullOrEmpty(url);
        var sappSdk = new SappStackSdk();
        services.AddSingleton(sappSdk);
        return services.AddSappClient(callerBuilder =>
        {
            var builder = callerBuilder.UseHttpClient(builder =>
            {
                builder.BaseAddress = url;
                builder.Configure = http =>
                {
                    http.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", sappSdk.UserAgent);
                };
            });
            if (callerAction != null)
                callerAction.Invoke(builder);
            else
                builder.UseAuthentication();
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
