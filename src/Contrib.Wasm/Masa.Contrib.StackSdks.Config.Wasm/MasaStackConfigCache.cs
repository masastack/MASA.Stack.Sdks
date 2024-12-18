﻿namespace Masa.Contrib.StackSdks.Config;

public class MasaStackConfigCache : ISingletonDependency
{
    public Dictionary<string, string> Section { get; private set; }
    private readonly IDccClient? _dccClient;

    public MasaStackConfigCache(IClientScopeServiceProviderAccessor serviceProviderAccessor)
    {
        _dccClient = serviceProviderAccessor.ServiceProvider.GetService<IDccClient>();
    }

    public virtual async Task InitializeAsync()
    {
        if (_dccClient != null)
        {
            Section = await _dccClient.OpenApiService.GetStackConfigAsync();
        }
    }
}