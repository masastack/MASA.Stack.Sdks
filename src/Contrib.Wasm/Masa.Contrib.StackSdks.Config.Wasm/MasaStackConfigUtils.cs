// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Config;

internal static class MasaStackConfigUtils
{
    public static async Task<Dictionary<string, string>> GetDefaultPublicConfigAsync(IDccClient client, string environment)
    {
        var publicConfig = await client.OpenApiService.GetStackConfigAsync(Configs.DEFAULT_CONFIG_NAME, environment);
        return publicConfig;
    }

    static string GetAppId(Dictionary<string, string> configMap, MasaStackProject project, MasaStackApp app)
    {
        var data = GetMasaStackJsonArray(configMap);
        return data.FirstOrDefault(i => i?["id"]?.ToString() == project.Name)?[app.Name]?["id"]?.ToString() ?? "";
    }

    static JsonArray GetMasaStackJsonArray(Dictionary<string, string> configMap)
    {
        var value = configMap.GetValueOrDefault(MasaStackConfigConstant.MASA_STACK) ?? "";
        return JsonSerializer.Deserialize<JsonArray>(value) ?? new();
    }
}
