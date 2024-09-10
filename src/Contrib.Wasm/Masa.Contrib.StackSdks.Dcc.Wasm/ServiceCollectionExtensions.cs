namespace Masa.Contrib.StackSdks.Dcc;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDccClient(this IServiceCollection services, string dccServiceBaseAddress)
    {
        MasaArgumentException.ThrowIfNullOrEmpty(dccServiceBaseAddress);

        return services.AddDccClient(callerBuilder =>
        {
            callerBuilder.UseHttpClient(builder =>
            {
                builder.BaseAddress = dccServiceBaseAddress;
            }).UseAuthentication();
        });
    }

    public static IServiceCollection AddDccClient(this IServiceCollection services, Func<string> dccServiceBaseAddressFunc)
    {
        MasaArgumentException.ThrowIfNull(dccServiceBaseAddressFunc);

        return services.AddDccClient(callerBuilder =>
        {
            callerBuilder.UseHttpClient(builder =>
            {
                builder.BaseAddress = dccServiceBaseAddressFunc.Invoke();
            }).UseAuthentication();
        });
    }

    public static IServiceCollection AddDccClient(this IServiceCollection services, Action<CallerBuilder> callerBuilder)
    {
        MasaArgumentException.ThrowIfNull(callerBuilder);

        if (services.Any(service => service.ServiceType == typeof(IDccClient)))
            return services;

        services.AddCaller(Constants.DEFAULT_CLIENT_NAME, callerBuilder.Invoke);

        services.AddScoped<IDccClient>(serviceProvider =>
        {
            var callProvider = serviceProvider.GetRequiredService<ICallerFactory>().Create(Constants.DEFAULT_CLIENT_NAME);
            var environmentUserContext = serviceProvider.GetRequiredService<IMultiEnvironmentUserContext>();
            var masaStackConfig = serviceProvider.GetRequiredService<IMasaStackConfig>();
            var dccCaching = new DccClient(callProvider, environmentUserContext, masaStackConfig);
            return dccCaching;
        });

        MasaApp.TrySetServiceCollection(services);
        return services;
    }
}
