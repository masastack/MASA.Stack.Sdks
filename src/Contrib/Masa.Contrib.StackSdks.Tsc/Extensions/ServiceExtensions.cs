// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

[assembly: InternalsVisibleTo("Masa.Contrib.StackSdks.Tsc.Tests")]

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceExtensions
{
    private const string DEFAULT_CLIENT_NAME = "masa.contrib.stacksdks.tsc";

    public static IServiceCollection AddTscClient(this IServiceCollection services, string tscServiceBaseUrl, Action<IMasaCallerClientBuilder>? callerAction = default)
    {
        ArgumentNullException.ThrowIfNull(tscServiceBaseUrl);

        if (services.Any(service => service.ServiceType == typeof(ITscClient)))
            return services;
        var tscSdk = new TscStackSdk();
        services.AddSingleton(tscSdk);
        services.AddCaller(DEFAULT_CLIENT_NAME, builder =>
        {
            var callBuilder = builder.UseHttpClient(options =>
            {
                options.BaseAddress = tscServiceBaseUrl;
                options.Configure = http =>
                {
                    http.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", tscSdk.UserAgent);
                };
            });
            if (callerAction == null)
                callBuilder.UseAuthentication();
            else
                callerAction.Invoke(callBuilder);
        });

        services.AddScoped<ITscClient>(serviceProvider =>
        {
            var caller = serviceProvider.GetRequiredService<ICallerFactory>().Create(DEFAULT_CLIENT_NAME);
            return new TscClient(caller);
        });

        MasaApp.TrySetServiceCollection(services);
        return services;
    }
}
