namespace Masa.Contrib.StackSdks.Tsc.OpenTelemetry.Traceing.Filters;

internal static class BlazorFilterExtenistion
{
    public static void AddBlazorFilter(this Action<AspNetCoreTraceInstrumentationOptions> options,
        OpenTelemetryInstrumentationOptions openTelemetryInstrumentationOptions,
        bool isInterruptSignalrTracing)
    {
        //FilterConsts.IsInterruptSignalrTracing = isInterruptSignalrTracing;
        options += opt => opt.Filter = IsBlazorFilter;
        openTelemetryInstrumentationOptions.AspNetCoreInstrumentationOptions += options;
    }

    internal static bool IsBlazorFilter(HttpContext httpContext)
    {
        return AspNetCoreFilterExtenistion.IsAspNetCoreFilter(httpContext) && !AspNetCoreFilterExtenistion.IsReuqestPathMatchHttpRequestPrefix(httpContext, FilterConsts._BlazorIgnorePrefix);
    }
}
