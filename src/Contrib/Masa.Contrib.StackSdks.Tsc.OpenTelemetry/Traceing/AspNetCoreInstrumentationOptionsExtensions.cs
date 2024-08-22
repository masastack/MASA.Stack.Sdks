// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Microsoft.Extensions.DependencyInjection;

public static class AspNetCoreInstrumentationOptionsExtensions
{
    private static readonly List<string> _CommonFilterIgnoreSuffix = new()
    {
        ".js",
        ".css",
        ".ico",
        ".png",
        ".woff",
        ".icon"
    };

    private static readonly List<string> _CommonFilterIgnorePrefix = new()
    {
        "/swagger",
        "/healthz"
    };

    private static readonly List<string> _BlazorFilterIgnorePrefix = new()
    {
        "/_blazor",
        "/_content",
        "/negotiate"
    };

    private static bool IsInterruptSignalrTracing = true;

    /// <summary>
    /// The default filter ignore list includes swagger.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="openTelemetryInstrumentationOptions"></param>
    /// <param name="isInterruptSignalrTracing"></param>
    public static void AppendDefaultFilter(this Action<AspNetCoreTraceInstrumentationOptions> options,
        OpenTelemetryInstrumentationOptions openTelemetryInstrumentationOptions,
        bool isInterruptSignalrTracing)
    {
        IsInterruptSignalrTracing = isInterruptSignalrTracing;
        options += opt =>
        {
            opt.Filter = IsDefaultFilter;
        };
        openTelemetryInstrumentationOptions.AspNetCoreInstrumentationOptions += options;
    }

    private static bool IsDefaultFilter(HttpContext httpContext) => !(IsInterruptSignalrTracing && IsWebsocket(httpContext)
                 || IsReuqestPathMatchHttpRequestPrefix(httpContext, _CommonFilterIgnorePrefix)
                 || IsReuqestPathMatchHttpRequestSuffix(httpContext, _CommonFilterIgnoreSuffix));

    /// <summary>
    /// The filter ignore list includes swagger and blazor and static files.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="openTelemetryInstrumentationOptions"></param>
    /// <param name="isInterruptSignalrTracing"></param>
    public static void AppendBlazorFilter(this Action<AspNetCoreTraceInstrumentationOptions> options,
        OpenTelemetryInstrumentationOptions openTelemetryInstrumentationOptions,
        bool isInterruptSignalrTracing)
    {
        IsInterruptSignalrTracing = isInterruptSignalrTracing;
        options += opt =>
        {
            opt.Filter = httpContext => IsDefaultFilter(httpContext) && IsBlazorFilter(httpContext);
        };
        openTelemetryInstrumentationOptions.AspNetCoreInstrumentationOptions += options;
    }


    public static void AppendHttpRequestFilter(this Action<HttpClientTraceInstrumentationOptions> options,
        OpenTelemetryInstrumentationOptions openTelemetryInstrumentationOptions,
        bool isInterruptSignalrTracing)
    {
        IsInterruptSignalrTracing = isInterruptSignalrTracing;
        options += opt =>
        {
            opt.FilterHttpRequestMessage = IsHttprquestFilter;
        };
        openTelemetryInstrumentationOptions.HttpClientInstrumentationOptions += options;
    }

    private static bool IsHttprquestFilter(HttpRequestMessage httpRequestMessage) => !(IsInterruptSignalrTracing &&
                 || IsReuqestPathMatchPrefix(httpRequestMessage, _CommonFilterIgnorePrefix)
                 || IsReuqestPathMatchSuffix(httpRequestMessage, _CommonFilterIgnoreSuffix));


    private static bool IsBlazorFilter(HttpContext httpContext) => !IsReuqestPathMatchHttpRequestPrefix(httpContext, _BlazorFilterIgnorePrefix);

    private static bool IsWebsocket(HttpContext httpContext)
    {
        if (httpContext.Request.Headers.ContainsKey("Connection") && httpContext.Request.Headers.ContainsKey(httpContext.Request.Headers["Connection"]!))
        {
            Activity.Current = null;
            return true;
        }
        return false;
    }

    private static bool IsReuqestPathMatchHttpRequestSuffix(HttpContext httpContext, List<string> suffix)
    {
        return !string.IsNullOrEmpty(httpContext.Request.Path.Value) && suffix.Exists(httpContext.Request.Path.Value.EndsWith);
    }

    private static bool IsReuqestPathMatchHttpRequestPrefix(HttpContext httpContext, List<string> prefix)
    {
        return !string.IsNullOrEmpty(httpContext.Request.Path.Value) && prefix.Exists(httpContext.Request.Path.Value.StartsWith);
    }

    private static bool IsReuqestPathMatchPrefix(HttpRequestMessage httpRequestMessage, List<string> prefix)
    {
        return httpRequestMessage.RequestUri != null && !string.IsNullOrEmpty(httpRequestMessage.RequestUri.PathAndQuery) && prefix.Exists(httpRequestMessage.RequestUri.PathAndQuery.StartsWith);
    }

    private static bool IsReuqestPathMatchSuffix(HttpRequestMessage httpRequestMessage, List<string> suffix)
    {
        return httpRequestMessage.RequestUri != null && !string.IsNullOrEmpty(httpRequestMessage.RequestUri.PathAndQuery) && suffix.Exists(httpRequestMessage.RequestUri.PathAndQuery.EndsWith);
    }
}
