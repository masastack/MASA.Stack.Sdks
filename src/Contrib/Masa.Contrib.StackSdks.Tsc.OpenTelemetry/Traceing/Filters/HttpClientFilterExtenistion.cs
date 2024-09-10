namespace Masa.Contrib.StackSdks.Tsc.OpenTelemetry.Traceing.Filters;

internal static class HttpClientFilterExtenistion
{
    public static void AddHttpClientFilter(this Action<HttpClientTraceInstrumentationOptions> options,
        OpenTelemetryInstrumentationOptions openTelemetryInstrumentationOptions,
        bool isInterruptSignalrTracing)
    {
        FilterConsts.IsInterruptSignalrTracing = isInterruptSignalrTracing;
        options += opt => opt.FilterHttpRequestMessage = IsHttpRequestMessageFilter;
        openTelemetryInstrumentationOptions.HttpClientInstrumentationOptions += options;
    }

    internal static bool IsHttpRequestMessageFilter(HttpRequestMessage httpRequestMessage) => !(FilterConsts.IsInterruptSignalrTracing && IsWebsocket(httpRequestMessage)
             || IsReuqestPathMatchHttpRequestSuffix(httpRequestMessage, FilterConsts.CommonIgnoreSuffix)
             || IsReuqestPathMatchPrefix(httpRequestMessage, FilterConsts.CommonIgnorePrefix)
             || IsReuqestPathMatch(httpRequestMessage, FilterConsts.CommonIgnore));

    internal static bool IsWebsocket(HttpRequestMessage httpRequestMessage)
    {
        var headers = httpRequestMessage.Headers;
        if (headers.Connection != null && headers.Upgrade != null)
        {
            Activity.Current = null;
            return true;
        }
        return false;
    }

    private static bool IsReuqestPathMatchHttpRequestSuffix(HttpRequestMessage httpRequestMessage, List<string> suffix)
    {
        return httpRequestMessage.RequestUri != null && !string.IsNullOrEmpty(httpRequestMessage.RequestUri.PathAndQuery) && suffix.Exists(httpRequestMessage.RequestUri.AbsolutePath.ToLower().EndsWith);
    }

    private static bool IsReuqestPathMatchPrefix(HttpRequestMessage httpRequestMessage, List<string> prefix)
    {
        return httpRequestMessage.RequestUri != null && !string.IsNullOrEmpty(httpRequestMessage.RequestUri.PathAndQuery) && prefix.Exists(httpRequestMessage.RequestUri.AbsolutePath.ToLower().StartsWith);
    }

    private static bool IsReuqestPathMatch(HttpRequestMessage httpRequestMessage, List<string> pathes)
    {
        return httpRequestMessage.RequestUri != null && !string.IsNullOrEmpty(httpRequestMessage.RequestUri.PathAndQuery) && pathes.Exists(httpRequestMessage.RequestUri.AbsolutePath.ToLower().Equals);
    }
}
