// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceExtensions
{
    public static IServiceCollection AddMasaTracing(this IServiceCollection services,
        Action<TracerProviderBuilder> builderConfigure,
        Action<OpenTelemetryInstrumentationOptions>? configure = null)
    {
        services.AddOpenTelemetry().AddMasaTracing(services, builderConfigure, configure, GetConfiguration(services));
        return services;
    }

    public static OpenTelemetryBuilder AddMasaTracing(this OpenTelemetryBuilder builder,
        IServiceCollection services,
        Action<TracerProviderBuilder> builderConfigure,
        Action<OpenTelemetryInstrumentationOptions>? openTelemetryInstrumentationOptions = null,
        IConfiguration? configuration = null)
    {
        configuration ??= GetConfiguration(services);
        FilterConsts.InitTraceFilter(configuration);
        return builder.WithTracing(builder =>
        {
            builder.SetSampler(new AlwaysOnSampler());
            var option = GetInstrumentationOptions(services, configuration);
            openTelemetryInstrumentationOptions?.Invoke(option);

            if (option.AspNetCoreInstrumentationOptions != null)
                builder.AddAspNetCoreInstrumentation(option.AspNetCoreInstrumentationOptions);

            if (option.HttpClientInstrumentationOptions != null)
                builder.AddHttpClientInstrumentation(option.HttpClientInstrumentationOptions);

            if (option.EntityFrameworkInstrumentationOptions != null)
                builder.AddEntityFrameworkCoreInstrumentation(option.EntityFrameworkInstrumentationOptions);

            if (option.ElasticsearchClientInstrumentationOptions != null)
                builder.AddElasticsearchClientInstrumentation(option.ElasticsearchClientInstrumentationOptions);

            if (option.ConnectionMultiplexerOptions != null)
            {
                foreach (Delegate handle in option.ConnectionMultiplexerOptions.GetInvocationList())
                {
                    var obj = handle.DynamicInvoke();
                    builder.AddRedisInstrumentation((IConnectionMultiplexer)obj!, options =>
                    {
                        options.SetVerboseDatabaseStatements = true;
                        option.StackExchangeRedisInstrumentationOptions?.Invoke(options);
                    });
                }
            }

            builderConfigure?.Invoke(builder);
            option.BuildTraceCallback?.Invoke(builder);
        });
    }

    private static IConfiguration? GetConfiguration(IServiceCollection services)
    {
        var descriptor = services.LastOrDefault(d => d.ServiceType == typeof(IConfiguration));
        return descriptor?.ImplementationInstance as IConfiguration;
    }

    private static OpenTelemetryInstrumentationOptions GetInstrumentationOptions(IServiceCollection services, IConfiguration? configuration)
    {
        var descriptor = services.LastOrDefault(d => d.ServiceType == typeof(OpenTelemetryInstrumentationOptions));
        if (descriptor?.ImplementationInstance is OpenTelemetryInstrumentationOptions options)
            return options;

        return new OpenTelemetryInstrumentationOptions(loggerFactory: services.BuildServiceProvider().GetRequiredService<ILoggerFactory>(), configuration);
    }
}