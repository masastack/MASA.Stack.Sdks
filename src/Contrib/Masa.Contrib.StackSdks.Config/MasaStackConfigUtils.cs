// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

[assembly: InternalsVisibleTo("Masa.Contrib.StackSdks.Config.Tests")]
namespace Masa.Contrib.StackSdks.Config;

internal static class MasaStackConfigUtils
{
    public static DccOptions GetDefaultDccOptions(Dictionary<string, string> configMap, MasaStackProject project, MasaStackApp app)
    {
        var data = GetMasaStackJsonArray(configMap);
        var dccServerAddress = data.FirstOrDefault(i => i?["id"]?.ToString() == MasaStackProject.DCC.Name)?[MasaStackApp.Service.Name]?["domain"]?.ToString() ?? "";
        var redisStr = configMap.GetValueOrDefault(MasaStackConfigConstant.REDIS) ?? throw new Exception("redis options can not null");
        var redis = JsonSerializer.Deserialize<RedisModel>(redisStr) ?? throw new JsonException();
        var secret = configMap.GetValueOrDefault(MasaStackConfigConstant.DCC_SECRET);

        var options = new DccOptions
        {
            Environment = configMap.GetValueOrDefault(MasaStackConfigConstant.ENVIRONMENT)!,
            ManageServiceAddress = dccServerAddress,
            RedisOptions = new Caching.Distributed.StackExchangeRedis.RedisConfigurationOptions
            {
                Servers = new List<Caching.Distributed.StackExchangeRedis.RedisServerOptions>
            {
                new Caching.Distributed.StackExchangeRedis.RedisServerOptions(redis.RedisHost,redis.RedisPort)
            },
                DefaultDatabase = redis.RedisDb,
                Password = redis.RedisPassword
            },
            PublicSecret = secret,
            ConfigObjectSecret = secret,
            AppId = GetAppId(configMap, project, app)
        };

        return options;
    }

    /// <summary>
    /// Build bootstrap options for Dapr Configuration.
    /// Local MASA_STACK / REDIS are optional: only Environment / Cluster / StoreName / AppId
    /// are required to read $public.DefaultConfig from the configuration store.
    /// </summary>
    public static DccDaprOptions GetDefaultDccDaprOptions(Dictionary<string, string> configMap, MasaStackProject project, MasaStackApp app)
    {
        var data = GetMasaStackJsonArray(configMap);
        var dccServerAddress = data.FirstOrDefault(i => i?["id"]?.ToString() == MasaStackProject.DCC.Name)?[MasaStackApp.Service.Name]?["domain"]?.ToString() ?? "";
        var secret = configMap.GetValueOrDefault(MasaStackConfigConstant.DCC_SECRET);
        var environment = FirstNonEmpty(
            configMap.GetValueOrDefault(MasaStackConfigConstant.ENVIRONMENT),
            System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
            "Development");
        var cluster = FirstNonEmpty(
            configMap.GetValueOrDefault(MasaStackConfigConstant.CLUSTER),
            "Default");

        return new DccDaprOptions
        {
            Environment = environment,
            Cluster = cluster,
            ManageServiceAddress = dccServerAddress,
            PublicSecret = secret,
            ConfigObjectSecret = secret,
            Secret = secret,
            StoreName = FirstNonEmpty(
                configMap.GetValueOrDefault(MasaStackConfigConstant.DCC_STORE_NAME),
                DEFAULT_DCC_STORE_NAME),
            AppId = GetAppId(configMap, project, app)
        };
    }

    static string GetAppId(Dictionary<string, string> configMap, MasaStackProject project, MasaStackApp app)
    {
        var data = GetMasaStackJsonArray(configMap);
        var appId = data.FirstOrDefault(i => i?["id"]?.ToString() == project.Name)?[app.Name]?["id"]?.ToString();
        if (!string.IsNullOrWhiteSpace(appId))
            return appId;

        // Fallback when local MASA_STACK is not provided; remote DefaultConfig can still override via GetValues().
        return $"{project.Name}-{app.Name}".ToLowerInvariant();
    }

    static JsonArray GetMasaStackJsonArray(Dictionary<string, string> configMap)
    {
        var value = configMap.GetValueOrDefault(MasaStackConfigConstant.MASA_STACK);
        if (string.IsNullOrWhiteSpace(value))
            return new JsonArray();

        try
        {
            return JsonSerializer.Deserialize<JsonArray>(value) ?? new JsonArray();
        }
        catch (JsonException)
        {
            return new JsonArray();
        }
    }

    static string FirstNonEmpty(params string?[] values)
        => values.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v)) ?? string.Empty;
}
