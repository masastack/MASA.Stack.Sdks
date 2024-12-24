// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMcClient(this IServiceCollection services, string mcServiceBaseAddress)
    {
        return services.AddMcClient(() => mcServiceBaseAddress);
    }

    public static IServiceCollection AddMcClient(this IServiceCollection services, Func<string> mcServiceBaseAddressFunc)
    {
        MasaArgumentException.ThrowIfNull(mcServiceBaseAddressFunc);
        var url = mcServiceBaseAddressFunc.Invoke();
        MasaArgumentException.ThrowIfNullOrEmpty(url);
        var mcSdk = new McStackSdk();
        services.AddSingleton(mcSdk);
        return services.AddMcClient(callerBuilder =>
        {
            callerBuilder
                .UseHttpClient(builder =>
                {
                    builder.BaseAddress = url;
                    builder.Configure = http =>
                    {
                        http.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", mcSdk.UserAgent);
                    };
                })
                .UseAuthentication();
        });
    }

    public static IServiceCollection AddMcClient(this IServiceCollection services, Action<CallerBuilder> callerBuilder)
    {
        MasaArgumentException.ThrowIfNull(callerBuilder);

        if (services.Any(service => service.ServiceType == typeof(IMcClient)))
            return services;

        services.AddCaller(DEFAULT_CLIENT_NAME, callerBuilder);

        services.AddScoped<IMcClient>(serviceProvider =>
        {
            var caller = serviceProvider.GetRequiredService<ICallerFactory>().Create(DEFAULT_CLIENT_NAME);
            var mcCaching = new McClient(caller);
            return mcCaching;
        });

        MasaApp.TrySetServiceCollection(services);
        return services;
    }
}
