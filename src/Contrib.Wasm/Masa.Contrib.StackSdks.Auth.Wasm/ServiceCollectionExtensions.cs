// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthClient(this IServiceCollection services, IConfiguration configuration,
        string? authServiceBaseAddress = null)
    {
        authServiceBaseAddress ??= configuration.GetValue<string>("$public.AppSettings:AuthClient:Url");
        services.AddAuthClient(authServiceBaseAddress!);

        return services;
    }

    public static IServiceCollection AddAuthClient(this IServiceCollection services, string authServiceBaseAddress)
    {
        MasaArgumentException.ThrowIfNullOrEmpty(authServiceBaseAddress);

        return services.AddAuthClient(callerBuilder =>
        {
            callerBuilder
                .UseHttpClient(builder => builder.BaseAddress = authServiceBaseAddress)
                .AddMiddleware<EnvironmentCallerMiddleware>()
                .UseAuthentication();
        });
    }

    private static IServiceCollection AddAuthClient(this IServiceCollection services, Action<CallerBuilder> callerBuilder)
    {
        MasaArgumentException.ThrowIfNull(callerBuilder);
        if (services.All(service => service.ServiceType != typeof(IMultiEnvironmentUserContext)))
            throw new Exception("Please add IMultiEnvironmentUserContext first.");

        services.AddCaller(DEFAULT_CLIENT_NAME, callerBuilder);

        services.AddScoped<IAuthClient>(serviceProvider =>
        {
            var userContext = serviceProvider.GetRequiredService<IMultiEnvironmentUserContext>();
            var caller = serviceProvider.GetRequiredService<ICallerFactory>().Create(DEFAULT_CLIENT_NAME);
            var authClient = new AuthClient(caller, userContext);
            return authClient;
        });
        services.AddScoped<IThirdPartyIdpService>(serviceProvider =>
        {
            var callProvider = serviceProvider.GetRequiredService<ICallerFactory>().Create(DEFAULT_CLIENT_NAME);
            var thirdPartyIdpService = new ThirdPartyIdpService(callProvider);
            return thirdPartyIdpService;
        });

        MasaApp.TrySetServiceCollection(services);

        return services;
    }

    public static IServiceCollection AddSsoClient(this IServiceCollection services, string ssoServiceAddress)
    {
        services.AddHttpClient(DEFAULT_SSO_CLIENT_NAME, httpClient =>
        {
            httpClient.BaseAddress = new Uri(ssoServiceAddress);
        });
        services.AddSingleton<ISsoClient, SsoClient>();

        return services;
    }
}
