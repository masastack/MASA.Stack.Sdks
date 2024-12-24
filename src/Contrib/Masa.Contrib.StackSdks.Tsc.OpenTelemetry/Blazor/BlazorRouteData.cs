namespace Masa.Contrib.StackSdks.Tsc.OpenTelemetry.Blazor;

internal class BlazorRouteData
{
    public string Template { get; set; } = default!;

    public Type Type { get; set; } = default!;

    public List<BlazorRouteParamKey> RouteKeies { get; set; } = new();

    public int MinCount => RouteKeies.Count(k => !k.CanNull);

    public int MaxCount => RouteKeies.Count;

    public bool IsMatch(string url)
    {
        if (string.IsNullOrEmpty(url))
            return false;
        var paths = url[1..].Split('/');
        if (!(MinCount - paths.Length == 0 || MaxCount - paths.Length == 0))
            return false;
        var index = 0;
        foreach (var param in paths)
        {
            if (RouteKeies[index].IsParamter)
                continue;
            if (!string.Equals(RouteKeies[index].Value, param, StringComparison.CurrentCultureIgnoreCase))
                return false;
            index++;
        }
        return true;
    }
}