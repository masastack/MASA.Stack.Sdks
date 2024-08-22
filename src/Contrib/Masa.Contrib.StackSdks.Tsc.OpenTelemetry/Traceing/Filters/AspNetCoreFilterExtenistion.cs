namespace Masa.Contrib.StackSdks.Tsc.OpenTelemetry.Traceing.Filters;

internal static class AspNetCoreFilterExtenistion
{
    public static void AddAspNetCoreFilter(this Action<AspNetCoreTraceInstrumentationOptions> options,
        OpenTelemetryInstrumentationOptions openTelemetryInstrumentationOptions,
        bool isInterruptSignalrTracing)
    {
        //FilterConsts.IsInterruptSignalrTracing = isInterruptSignalrTracing;
        options += opt => opt.Filter = IsAspNetCoreFilter;
        openTelemetryInstrumentationOptions.AspNetCoreInstrumentationOptions += options;
    }

    public static bool IsAspNetCoreFilter(HttpContext httpContext) => !(FilterConsts.IsInterruptSignalrTracing && IsWebsocket(httpContext)
             || IsReuqestPathMatchHttpRequestPrefix(httpContext, FilterConsts._CommonIgnorePrefix)
             || IsReuqestPathMatchHttpRequestSuffix(httpContext, FilterConsts._CommonIgnoreSuffix));

    internal static bool IsWebsocket(HttpContext httpContext)
    {
        if (httpContext.Request.Headers.ContainsKey("Connection") && httpContext.Request.Headers.ContainsKey(httpContext.Request.Headers["Connection"]!))
        {
            Activity.Current = null;
            return true;
        }
        return false;
    }

    internal static bool IsReuqestPathMatchHttpRequestSuffix(HttpContext httpContext, List<string> suffix)
    {
        return !string.IsNullOrEmpty(httpContext.Request.Path.Value) && suffix.Exists(httpContext.Request.Path.Value.EndsWith);
    }

    internal static bool IsReuqestPathMatchHttpRequestPrefix(HttpContext httpContext, List<string> prefix)
    {
        return !string.IsNullOrEmpty(httpContext.Request.Path.Value) && prefix.Exists(httpContext.Request.Path.Value.StartsWith);
    }

}
