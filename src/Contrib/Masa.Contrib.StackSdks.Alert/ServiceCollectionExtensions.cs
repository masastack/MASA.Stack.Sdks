// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAlertClient(this IServiceCollection services, string alertServiceBaseAddress)
    {
        return services.AddAlertClient(() => alertServiceBaseAddress);
    }

    public static IServiceCollection AddAlertClient(this IServiceCollection services, Func<string> alertServiceBaseAddressFunc)
    {
        MasaArgumentException.ThrowIfNull(alertServiceBaseAddressFunc);
        var url = alertServiceBaseAddressFunc.Invoke();
        MasaArgumentException.ThrowIfNullOrEmpty(url);
        var alertSdk = new AlertStackSdk();
        services.AddSingleton(alertSdk);
        return services.AddAlertClient(callerBuilder =>
        {
            callerBuilder
                .UseHttpClient(builder =>
                {
                    builder.BaseAddress = url;
                    MasaArgumentException.ThrowIfNull(builder.BaseAddress);
                    builder.Configure = http =>
                    {
                        http.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", alertSdk.UserAgent);
                    };
                })
                .UseAuthentication();
        });
    }

    public static IServiceCollection AddAlertClient(this IServiceCollection services, Action<CallerBuilder> callerBuilder)
    {
        MasaArgumentException.ThrowIfNull(callerBuilder);

        if (services.Any(service => service.ServiceType == typeof(IAlertClient)))
            return services;

        services.AddCaller(DEFAULT_CLIENT_NAME, callerBuilder);

        services.AddScoped<IAlertClient>(serviceProvider =>
        {
            var caller = serviceProvider.GetRequiredService<ICallerFactory>().Create(DEFAULT_CLIENT_NAME);
            var alertCaching = new AlertClient(caller);
            return alertCaching;
        });

        MasaApp.TrySetServiceCollection(services);
        return services;
    }
}
