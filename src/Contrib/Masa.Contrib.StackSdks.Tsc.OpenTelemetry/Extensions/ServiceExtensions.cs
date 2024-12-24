// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

[assembly: InternalsVisibleTo("Masa.Contrib.StackSdks.Tsc.OpenTelemetry.Tests")]

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceExtensions
{
    public static IServiceCollection AddObservable(this IServiceCollection services,
        ILoggingBuilder loggingBuilder,
        IConfiguration configuration,
        bool isBlazor = false,
        bool isInterruptSignalRTracing = true,
        IEnumerable<string>? activitySources = default)
    {
        return services.AddObservable(loggingBuilder,
            configuration.GetSection("Masa:Observable").Get<MasaObservableOptions>()!,
            configuration.GetSection("Masa:Observable:OtlpUrl").Get<string>(),
            isBlazor,
            isInterruptSignalRTracing,
            activitySources);
    }

    public static IServiceCollection AddObservable(this IServiceCollection services,
        ILoggingBuilder loggingBuilder,
        Func<MasaObservableOptions> optionsConfigure,
        Func<string>? otlpUrlConfigure = null,
        bool isBlazor = false,
        bool isInterruptSignalRTracing = true,
        IEnumerable<string>? activitySources = default)
    {
        ArgumentNullException.ThrowIfNull(optionsConfigure);
        var options = optionsConfigure();
        var otlpUrl = otlpUrlConfigure?.Invoke() ?? string.Empty;
        return services.AddObservable(loggingBuilder, options, otlpUrl, isBlazor, isInterruptSignalRTracing, activitySources);
    }

    public static IServiceCollection AddObservable(this IServiceCollection services,
        ILoggingBuilder loggingBuilder,
        MasaObservableOptions option,
        string? otlpUrl = null,
        bool isBlazor = false,
        bool isInterruptSignalRTracing = true,
        IEnumerable<string>? activitySources = default,
        IEnumerable<Assembly>? blazorRouteAssemblies = default)
    {
        ArgumentNullException.ThrowIfNull(option);

        Uri? uri = null;
        if (!string.IsNullOrEmpty(otlpUrl) && !Uri.TryCreate(otlpUrl, UriKind.Absolute, out uri))
            throw new UriFormatException($"{nameof(otlpUrl)}:{otlpUrl} is invalid url");
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddMasaService(option))
            .AddMasaTracing(services, builder => AddTraceOtlpExporter(builder, uri!, activitySources?.ToArray()),
            builder =>
            {
                if (isBlazor)
                    builder.AspNetCoreInstrumentationOptions.AddBlazorFilter(builder, isInterruptSignalRTracing);
                else
                    builder.AspNetCoreInstrumentationOptions.AddAspNetCoreFilter(builder, isInterruptSignalRTracing);

                builder.HttpClientInstrumentationOptions.AddHttpClientFilter(builder, isInterruptSignalRTracing);
            })
            .AddMasaMetrics(builder => AddMetricOtlpExporter(builder, uri!, activitySources?.ToArray()));

        var resources = ResourceBuilder.CreateDefault().AddMasaService(option);
        loggingBuilder.AddMasaOpenTelemetry(builder =>
        {
            builder.SetResourceBuilder(resources);
            builder.AddOtlpExporter(otlp => otlp.Endpoint = uri!);
        });
        if (blazorRouteAssemblies != null && blazorRouteAssemblies.Any())
        {
            BlazorRouteManager.InitRoute(blazorRouteAssemblies.ToArray());
        }
        return services;
    }

    private static void AddTraceOtlpExporter(TracerProviderBuilder builder, Uri uri, string[]? activitySources = default)
    {
        if (activitySources != null && activitySources.Any())
            builder.AddSource(activitySources.ToArray());
        builder.AddOtlpExporter(options => options.Endpoint = uri);
    }

    private static void AddMetricOtlpExporter(MeterProviderBuilder builder, Uri uri, string[]? activitySources = default)
    {
        if (activitySources != null && activitySources.Any())
            builder.AddMeter(activitySources.ToArray());
        builder.AddOtlpExporter(options => options.Endpoint = uri);
    }

    public static IApplicationBuilder UseMASAHttpReponseLog(this IApplicationBuilder app)
    {
        return app.UseMiddleware<HttpResponseMiddleware>();
    }
}
