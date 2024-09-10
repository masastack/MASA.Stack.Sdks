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
        masaCallerClientBuilder.Services.TryAddScoped<TokenProvider>();
        masaCallerClientBuilder.Services.TryAddScoped<IMultiEnvironmentContext>(sp =>
        {
            var multiEnvironmentContext = new MultiEnvironmentContext();
            return multiEnvironmentContext;
        });
        masaCallerClientBuilder.UseAuthentication(serviceProvider =>
            new AuthenticationService(
                serviceProvider.GetRequiredService<TokenProvider>(),
                serviceProvider.GetRequiredService<IMultiEnvironmentContext>()
            ));
        return masaCallerClientBuilder;
    }
}
