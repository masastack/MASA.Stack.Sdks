namespace Masa.Contrib.StackSdks.Tsc.OpenTelemetry.Blazor;

internal class BlazorRouteParamKey
{
    public string Value { get; set; } = default!;

    public int Index { get; set; }

    public bool IsParamter { get; set; }

    public bool CanNull { get; set; }
}
