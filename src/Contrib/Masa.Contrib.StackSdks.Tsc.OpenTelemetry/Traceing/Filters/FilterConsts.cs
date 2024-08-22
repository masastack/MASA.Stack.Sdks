namespace Masa.Contrib.StackSdks.Tsc.OpenTelemetry.Traceing.Filters;

internal static class FilterConsts
{
    internal static readonly List<string> _CommonIgnoreSuffix = new()
{
    ".js",
    ".css",
    ".ico",
    ".png",
    ".woff",
    ".icon"
};

    internal static readonly List<string> _CommonIgnorePrefix = new()
{
    "/swagger",
    "/healthz"
};

    internal static readonly List<string> _BlazorIgnorePrefix = new()
{
    "/_blazor",
    "/_content",
    "/negotiate"
};
    internal static bool IsInterruptSignalrTracing { get; private set; } = false;
}
