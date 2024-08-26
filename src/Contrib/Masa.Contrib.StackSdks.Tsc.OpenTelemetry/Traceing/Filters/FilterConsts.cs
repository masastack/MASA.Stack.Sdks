namespace Masa.Contrib.StackSdks.Tsc.OpenTelemetry.Traceing.Filters;

internal static class FilterConsts
{
    private const string MASASTACK_TRACE_IGNORE_PATH_SUFFIX = nameof(MASASTACK_TRACE_IGNORE_PATH_SUFFIX);

    private const string MASASTACK_TRACE_IGNORE_PATH_PREFIX = nameof(MASASTACK_TRACE_IGNORE_PATH_PREFIX);

    private const string MASASTACK_TRACE_IGNORE_PATH = nameof(MASASTACK_TRACE_IGNORE_PATH);

    public static List<string> CommonIgnoreSuffix { get; private set; } = new()
{
    ".js",
    ".css",
    ".ico",
    ".png",
    ".woff",
    ".icon"
};

    public static List<string> CommonIgnorePrefix { get; private set; } = new()
{
    "/swagger",
    "/healthz"
};

    public static List<string> CommonIgnore { get; private set; } = new();

    public static List<string> BlazorIgnorePrefix { get; private set; } = new()
{
    "/_blazor",
    "/_content",
    "/negotiate"
};

    public static bool IsInterruptSignalrTracing { get; set; } = false;

    public static void InitTraceFilter(IConfiguration configuration)
    {
        var prefixes = GetValues(configuration, MASASTACK_TRACE_IGNORE_PATH_PREFIX);
        if (prefixes != null && prefixes.Length > 0) SetValues(CommonIgnorePrefix, prefixes!);

        var suffixes = GetValues(configuration, MASASTACK_TRACE_IGNORE_PATH_SUFFIX);
        if (suffixes != null && suffixes.Length > 0) SetValues(CommonIgnoreSuffix, suffixes!);

        var pathes = GetValues(configuration, MASASTACK_TRACE_IGNORE_PATH);
        if (pathes != null && pathes.Length > 0) SetValues(CommonIgnore, pathes!);
    }

    private static string[]? GetValues(IConfiguration configuration, string key)
    {
        try
        {
            return configuration.GetValue<string[]>(key);
        }
        catch
        {
            return default;
        }
    }

    private static void SetValues(List<string> targets, string[] values)
    {
        targets.AddRange(values.Where(value => !targets.Contains(value.ToLower())));
    }
}
