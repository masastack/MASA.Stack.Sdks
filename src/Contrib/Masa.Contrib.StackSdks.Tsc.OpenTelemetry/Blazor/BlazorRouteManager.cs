namespace Masa.Contrib.StackSdks.Tsc.OpenTelemetry.Blazor;

public sealed class BlazorRouteManager
{
    private BlazorRouteManager() { }

    private static Dictionary<Type, List<BlazorRouteData>> _routes = new();

    internal static bool EnableBlazorRoute { get; private set; }

    public static void InitRoute(params Assembly[] assemblies)
    {
        if (assemblies == null || assemblies.Length == 0)
            throw new ArgumentNullException(nameof(assemblies));

        EnableBlazorRoute = true;
        foreach (var assembly in assemblies)
        {
            InitRoute(assembly);
        }
    }

    private static void InitRoute(Assembly assembly)
    {
        if (assembly == null)
            return;

        var typeRoutes = assembly.GetTypes().Select(type => new { type, routes = type.GetCustomAttributes<RouteAttribute>().ToList() }).Where(item => item.routes.Count > 0).ToList();

        foreach (var item in typeRoutes)
        {
            if (_routes.ContainsKey(item.type))
                continue;
            var routeDatas = item.routes.Select(route =>
            {
                var values = (route.Template.StartsWith('/') ? route.Template[1..] : route.Template).Split('/');
                return new BlazorRouteData
                {
                    Template = route.Template,
                    Type = item.type,
                    RouteKeies = values.Select((value, index) => new BlazorRouteParamKey
                    {
                        Value = value,
                        Index = index,
                        IsParamter = IsParamter(value),
                        CanNull = CanNull(value, index == values.Length - 1)
                    }
                    ).ToList()
                };
            }).ToList();
            _routes.Add(item.type, routeDatas);
        }
    }

    internal static bool TryGetUrlRoute(string url, out BlazorRouteData? route)
    {
        route = default;
        foreach (var type in _routes.Keys)
        {
            var routes = _routes[type];
            var matches = routes.Where(route => route.IsMatch(url)).OrderByDescending(item => item.MaxCount);
            if (matches.Any())
            {
                route = matches.First();
                return true;
            }
        }
        return false;
    }

    private static bool IsParamter(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(nameof(value));
        return Regex.IsMatch(value, @"^\{\w+:?\w+\??\}$");
    }

    private static bool CanNull(string value, bool isLast)
    {
        return isLast && value.Contains('?');
    }
}