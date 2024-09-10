namespace Masa.Contrib.StackSdks.Caller;

public class AuthenticationService : IAuthenticationService
{
    private readonly TokenProvider _tokenProvider;
    private readonly IMultiEnvironmentContext _multiEnvironmentContext;

    public AuthenticationService(TokenProvider tokenProvider,
        IMultiEnvironmentContext multiEnvironmentContext)
    {
        _tokenProvider = tokenProvider;
        _multiEnvironmentContext = multiEnvironmentContext;
    }

    public async Task ExecuteAsync(HttpRequestMessage requestMessage)
    {
        var accessToken = await _tokenProvider.GetAccessTokenAsync();
        if (!accessToken.IsNullOrWhiteSpace())
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        if (!string.IsNullOrEmpty(_multiEnvironmentContext.CurrentEnvironment) && !requestMessage.Headers.Any(x => x.Key == IsolationConsts.ENVIRONMENT))
            requestMessage.Headers.Add(IsolationConsts.ENVIRONMENT, _multiEnvironmentContext.CurrentEnvironment);
    }
}
