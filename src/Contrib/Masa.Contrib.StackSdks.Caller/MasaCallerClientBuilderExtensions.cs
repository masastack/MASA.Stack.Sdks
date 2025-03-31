// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Microsoft.Extensions.DependencyInjection;

public static class MasaCallerClientBuilderExtensions
{
    /// <summary>
    /// Caller adds default authentication
    /// </summary>
    /// <param name="masaCallerClientBuilder"></param>
    /// <param name="defaultScheme">The default scheme used as a fallback for all other schemes.</param>
    /// <returns></returns>
    public static IMasaCallerClientBuilder UseAuthentication(
        this IMasaCallerClientBuilder masaCallerClientBuilder)
    {
        return masaCallerClientBuilder.UseClientAuthentication<DefaultTokenGenerater>();
    }

    public static IMasaCallerClientBuilder UseClientAuthentication(
        this IMasaCallerClientBuilder masaCallerClientBuilder, string clientId, string ssoHost, string? multiLevelCacheName = default, params string[] scopes)
    {
        ArgumentNullException.ThrowIfNull(clientId);
        ArgumentNullException.ThrowIfNull(ssoHost);
        ClientTokenGenerater.ClientId = clientId;
        ClientTokenGenerater.SsoDomain = ssoHost;
        if (!string.IsNullOrEmpty(multiLevelCacheName))
            ClientTokenGenerater.CacheClientName = multiLevelCacheName;
        if (scopes != null && scopes.Length > 0 && scopes.Any(s => !string.IsNullOrEmpty(s)))
            ClientTokenGenerater.Scopes = scopes;

        masaCallerClientBuilder.Services.AddMemoryCache();
        return masaCallerClientBuilder.UseClientAuthentication<ClientTokenGenerater>();
    }

    private static IMasaCallerClientBuilder UseClientAuthentication<T>(
        this IMasaCallerClientBuilder masaCallerClientBuilder) where T : class, ITokenGenerater
    {
        masaCallerClientBuilder.Services.AddHttpContextAccessor();
        masaCallerClientBuilder.Services.TryAddScoped<ITokenGenerater, T>();
        masaCallerClientBuilder.Services.TryAddScoped(s => s.GetRequiredService<ITokenGenerater>().Generater());
        masaCallerClientBuilder.Services.TryAddScoped<MultiEnvironmentContext>();
        masaCallerClientBuilder.Services.TryAddScoped(typeof(IMultiEnvironmentContext), s => s.GetRequiredService<MultiEnvironmentContext>());
        masaCallerClientBuilder.UseAuthentication(serviceProvider =>
            new AuthenticationService(
                serviceProvider.GetRequiredService<TokenProvider>(),
                null,
                serviceProvider.GetRequiredService<IMultiEnvironmentContext>()
            ));
        return masaCallerClientBuilder;
    }
}
