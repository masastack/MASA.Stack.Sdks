// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using Masa.BuildingBlocks.StackSdks.Config.Consts;

namespace System.Diagnostics;

public static class ActivityExtension
{
    public static Activity AddMasaHttpRequest(this Activity activity, HttpRequest httpRequest)
    {
        activity.SetTag(OpenTelemetryAttributeName.Http.FLAVOR, httpRequest.Protocol);
        activity.SetTag(OpenTelemetryAttributeName.Http.SCHEME, httpRequest.Scheme);
        activity.SetTag(OpenTelemetryAttributeName.Http.REQUEST_CONTENT_LENGTH, httpRequest.ContentLength);
        activity.SetTag(OpenTelemetryAttributeName.Http.REQUEST_CONTENT_TYPE, httpRequest.ContentType);
        if (httpRequest.Headers != null)
        {
            activity.SetTag(OpenTelemetryAttributeName.Http.REQUEST_AUTHORIZATION, httpRequest.Headers.Authorization);
            activity.SetTag(OpenTelemetryAttributeName.Http.REQUEST_USER_AGENT, httpRequest.Headers.UserAgent);
            activity.SetTag(OpenTelemetryAttributeName.Http.CLIENT_IP, GetIp(httpRequest.Headers, httpRequest.HttpContext!.Connection.RemoteIpAddress));
        }

        SetUserInfo(activity, httpRequest.HttpContext.User);
        if (httpRequest.Body != null)
        {
            if (!httpRequest.Body.CanSeek)
                httpRequest.EnableBuffering();
            SetActivityBody(activity, httpRequest.Body, GetHttpRequestEncoding(httpRequest)).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        activity.SetTag(OpenTelemetryAttributeName.Host.NAME, Dns.GetHostName());
        return activity;
    }

    private static string GetIp(IHeaderDictionary headers, IPAddress? deafultIp)
    {
        if (headers.TryGetValue("X-Forwarded-For", out StringValues value))
        {
            var ip = value.ToString().Split(',')[0].Trim();
            if (ip.Length > 0) return ip;
        }
        if (headers.TryGetValue("X-Real-IP", out value))
        {
            var ip = value.ToString();
            if (ip.Length > 0) return ip;
        }

        return deafultIp?.ToString() ?? string.Empty;
    }

    public static Activity AddMasaHttpRequestMessage(this Activity activity, HttpRequestMessage httpRequest)
    {
        activity.SetTag(OpenTelemetryAttributeName.Http.SCHEME, httpRequest.RequestUri?.Scheme);
        activity.SetTag(OpenTelemetryAttributeName.Host.NAME, Dns.GetHostName());
        if (httpRequest.Headers != null)
        {
            activity.SetTag(OpenTelemetryAttributeName.Http.REQUEST_AUTHORIZATION, httpRequest.Headers.Authorization);
            activity.SetTag(OpenTelemetryAttributeName.Http.REQUEST_USER_AGENT, httpRequest.Headers.UserAgent);
        }
        if (httpRequest.Content != null)
        {
            SetActivityBody(activity,
                httpRequest.Content.ReadAsStreamAsync().ConfigureAwait(false).GetAwaiter().GetResult(),
                GetHttpRequestMessageEncoding(httpRequest)).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        return activity;
    }

    public static Activity AddMasaHttpResponse(this Activity activity, HttpResponse httpResponse)
    {
        activity.SetTag(OpenTelemetryAttributeName.Http.RESPONSE_CONTENT_LENGTH, httpResponse.ContentLength);
        activity.SetTag(OpenTelemetryAttributeName.Http.RESPONSE_CONTENT_TYPE, httpResponse.ContentType);
        activity.SetTag(OpenTelemetryAttributeName.Host.NAME, Dns.GetHostName());

        SetUserInfo(activity, httpResponse.HttpContext.User);
        if (httpResponse.HttpContext.Request != null && httpResponse.HttpContext.Request.Headers != null)
        {
            activity.SetTag(OpenTelemetryAttributeName.Http.CLIENT_IP, GetIp(httpResponse.HttpContext.Request.Headers, httpResponse.HttpContext!.Connection.RemoteIpAddress));
        }
        activity.SetStatus(GetStatusResult(httpResponse.StatusCode));
        return activity;
    }

    public static Activity AddMasaHttpResponseMessage(this Activity activity, HttpResponseMessage httpResponse)
    {
        activity.SetTag(OpenTelemetryAttributeName.Host.NAME, Dns.GetHostName());
        activity.SetStatus(GetStatusResult((int)httpResponse.StatusCode));
        return activity;
    }

    private static Encoding? GetHttpRequestEncoding(HttpRequest httpRequest)
    {
        if (httpRequest.Body != null)
        {
            var contentType = httpRequest.ContentType;
            if (!string.IsNullOrEmpty(contentType)
                && MediaTypeHeaderValue.TryParse(contentType, out var attr)
                && attr != null
                && !string.IsNullOrEmpty(attr.CharSet))
                return Encoding.GetEncoding(attr.CharSet);
        }

        return null;
    }

    private static Encoding? GetHttpRequestMessageEncoding(HttpRequestMessage httpRequest)
    {
        if (httpRequest.Content != null)
        {
            var encodeStr = httpRequest.Content.Headers?.ContentType?.CharSet;

            if (!string.IsNullOrEmpty(encodeStr))
            {
                return Encoding.GetEncoding(encodeStr);
            }
        }

        return null;
    }

    private static async Task SetActivityBody(Activity activity, Stream inputSteam, Encoding? encoding = null)
    {
        (long length, string? body) = await inputSteam.ReadAsStringAsync(encoding);

        if (length <= 0)
            return;
        if (length - OpenTelemetryInstrumentationOptions.MaxBodySize > 0)
        {
            OpenTelemetryInstrumentationOptions.Logger?.LogInformation("Request body in base64 encode: {Body}", body);
        }
        else
        {
            activity.SetTag(OpenTelemetryAttributeName.Http.REQUEST_CONTENT_BODY, body);
        }
    }

    /// <summary>
    /// 存在坑，masaauth声明不规范导致，之后有调整了再调整此处
    /// </summary>
    /// <param name="activity"></param>
    /// <param name="claims"></param>
    private static void SetUserInfo(Activity activity, ClaimsPrincipal? claims)
    {
        if (claims == null || !claims.Claims.Any()) return;
        string userId = claims.FindFirstValue(IdentityClaimConsts.USER_ID) ?? claims.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        string userName = claims.FindFirstValue(IdentityClaimConsts.USER_NAME) ?? claims.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

        activity.AddTag(OpenTelemetryAttributeName.EndUser.ID, userId);
        activity.AddTag(OpenTelemetryAttributeName.EndUser.USER_NICK_NAME, userName);
    }

    private static ActivityStatusCode GetStatusResult(int statusCode)
    {
        if (statusCode - 200 >= 0 && statusCode - 299 <= 0 || statusCode - 599 == 0)
            return ActivityStatusCode.Ok;
        if (statusCode - HttpStatusCode.BadRequest >= 0 && statusCode - 599 < 0)
            return ActivityStatusCode.Error;

        return ActivityStatusCode.Unset;
    }
}
