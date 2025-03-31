namespace Masa.Contrib.StackSdks.Caller;

internal class ClientTokenGenerater : DefaultTokenGenerater
{
    internal static string SsoDomain { get; set; }
    internal static string ClientId { get; set; }
    internal static string[] Scopes { get; set; } = new string[] { "MasaStack" };

    internal static string CacheClientName { get; set; } = "masa.contrib.configuration.configurationapi.dcc";

    readonly IMemoryCache _memoryCache;
    readonly IMultilevelCacheClient? _multilevelCacheClient = default;
    readonly HttpClient _httpClient;

    public ClientTokenGenerater(IHttpContextAccessor httpContextAccessor, IServiceProvider service, IHttpClientFactory httpClientFactory) : base(httpContextAccessor)
    {
        _httpClient = httpClientFactory.CreateClient("clientTokenClient");
        var factory = service.GetService<IMultilevelCacheClientFactory>();
        if (factory != null)
            _multilevelCacheClient = factory.Create(CacheClientName);
        _memoryCache = service.GetRequiredService<IMemoryCache>();
    }

    public override TokenProvider Generater()
    {
        if (_httpContextAccessor.HttpContext == null)
            return CacheClientTokenAsync().ConfigureAwait(false).GetAwaiter().GetResult()!;

        return base.Generater();
    }

    private async Task<TokenProvider?> CacheClientTokenAsync()
    {
        if (_multilevelCacheClient != null)
            return await _multilevelCacheClient.GetOrSetAsync($"{ClientId}_token", async () =>
              {
                  var result = await GetClientTokenAsync();
                  return new CacheEntry<TokenProvider>(result, TimeSpan.FromSeconds(result.ExpiresIn));
              });

        return await _memoryCache.GetOrCreateAsync($"{ClientId}_token", async entry =>
          {
              var result = await GetClientTokenAsync();
              entry.AbsoluteExpiration = DateTime.Now.AddSeconds(result.ExpiresIn);
              entry.Value = result;
              return result;
          });
    }

    private async Task<TokenProvider> GetClientTokenAsync()
    {
        var request = new ClientCredentialsTokenRequest
        {
            Address = SsoDomain + "/connect/token",
            GrantType = GrantType.CLIENT_CREDENTIALS,
            ClientId = ClientId,
            Scope = string.Join(" ", Scopes)
        };
        var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(request);
        if (tokenResponse == null)
            throw new UserFriendlyException("request client credentials token failed");
        if (tokenResponse.IsError)
            throw new UserFriendlyException(tokenResponse.Error);
        var result = new TokenProvider
        {
            AccessToken = tokenResponse.AccessToken,
            RefreshToken = tokenResponse.RefreshToken,
            IdToken = tokenResponse.IdentityToken,
            ExpiresIn = tokenResponse.ExpiresIn
        };
        return result;
    }
}