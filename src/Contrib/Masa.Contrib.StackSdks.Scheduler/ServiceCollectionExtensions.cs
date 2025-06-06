// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSchedulerClient(this IServiceCollection services, string schedulerServiceBaseAddress, Action<IMasaCallerClientBuilder>? callerAction = default)
    {
        if (string.IsNullOrWhiteSpace(schedulerServiceBaseAddress))
        {
            throw new ArgumentNullException(nameof(schedulerServiceBaseAddress));
        }

        return services.AddSchedulerClient(callerBuilder =>
        {
            var builder = callerBuilder
                .UseHttpClient(builder =>
                {
                    builder.Configure = opt => opt.BaseAddress = new Uri(schedulerServiceBaseAddress);
                });
            if (callerAction != null)
                callerAction.Invoke(builder);
            else
                builder.UseAuthentication();
        });
    }

    public static IServiceCollection AddSchedulerClient(this IServiceCollection services, Action<CallerBuilder> callerBuilder)
    {
        ArgumentNullException.ThrowIfNull(callerBuilder, nameof(callerBuilder));

#if (NET8_0_OR_GREATER)
        if (services.Any(service => service.IsKeyedService == false && service.ImplementationType == typeof(SchedulerProvider)))
            return services;
#else
        if (services.Any(service => service.ImplementationType == typeof(SchedulerProvider)))
            return services;
#endif

        services.AddSingleton<SchedulerProvider>();
        services.AddCaller(DEFAULT_CLIENT_NAME, callerBuilder);

        services.AddScoped<ISchedulerClient>(serviceProvider =>
        {
            var caller = serviceProvider.GetRequiredService<ICallerFactory>().Create(DEFAULT_CLIENT_NAME);
            var schedulerClient = new SchedulerClient(caller);
            return schedulerClient;
        });

        MasaApp.TrySetServiceCollection(services);
        return services;
    }

    private sealed class SchedulerProvider
    {
    }
}
