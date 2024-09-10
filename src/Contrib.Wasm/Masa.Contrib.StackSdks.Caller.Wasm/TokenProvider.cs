// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Caller;

public class TokenProvider
{
    private readonly IAccessTokenProvider _accessTokenProvider;

    public TokenProvider(IAccessTokenProvider accessTokenProvider)
    {
        _accessTokenProvider = accessTokenProvider;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var tokenResult = await _accessTokenProvider.RequestAccessToken();
        if (tokenResult.TryGetToken(out var token))
        {
            return token.Value;
        }
        return string.Empty;
    }
}
