// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Config;

public class MasaStackConfig : IMasaStackConfig
{
    protected IClientScopeServiceProviderAccessor ClientScopeServiceProviderAccessor { get; }

    private static ConcurrentDictionary<string, string> ConfigMap { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public MasaStackConfig() { }

    public MasaStackConfig(IClientScopeServiceProviderAccessor clientScopeServiceProviderAccessor, Dictionary<string, string> configs)
    {
        ClientScopeServiceProviderAccessor = clientScopeServiceProviderAccessor;
        ConfigMap = new(configs);
    }

    public RedisModel RedisModel
    {
        get
        {
            var redisStr = GetValue(MasaStackConfigConstant.REDIS);
            return JsonSerializer.Deserialize<RedisModel>(redisStr) ?? throw new JsonException();
        }
    }

    public ElasticModel ElasticModel
    {
        get
        {
            var elasticStr = GetValue(MasaStackConfigConstant.ELASTIC);
            return JsonSerializer.Deserialize<ElasticModel>(elasticStr) ?? throw new JsonException();
        }
    }

    public bool IsDemo => bool.Parse(GetValue(MasaStackConfigConstant.IS_DEMO));

    public string Version => GetValue(MasaStackConfigConstant.VERSION);

    public string Cluster => GetValue(MasaStackConfigConstant.CLUSTER);

    public string OtlpUrl => GetValue(MasaStackConfigConstant.OTLP_URL);

    public string DomainName => GetValue(MasaStackConfigConstant.DOMAIN_NAME);

    public string Environment => GetValue(MasaStackConfigConstant.ENVIRONMENT);

    public string Namespace => GetValue(MasaStackConfigConstant.NAMESPACE);

    public string AdminPwd => GetValue(MasaStackConfigConstant.ADMIN_PWD);

    public string DccSecret => GetValue(MasaStackConfigConstant.DCC_SECRET);

    public string SuffixIdentity => GetValue(MasaStackConfigConstant.SUFFIX_IDENTITY);

    public List<string> GetProjectList() => this.GetMasaStack().Select(jNode => jNode!["id"]!.ToString()).ToList();

    public string GetValue(string key)
    {
        GetValues().TryGetValue(key, out var value);
        return value ?? ConfigMap[key];
    }

    public virtual Dictionary<string, string> GetValues()
    {
        if (ClientScopeServiceProviderAccessor is not null && ClientScopeServiceProviderAccessor.ServiceProvider is not null)
        {
            var masaStackConfigCache = ClientScopeServiceProviderAccessor.ServiceProvider.GetRequiredService<MasaStackConfigCache>();
            return masaStackConfigCache?.Section ?? new();
        }
        return new();
    }
}
