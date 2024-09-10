// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Tsc.OpenTelemetry.Tracing.Handler;

internal class ExceptionHandler
{
    public static string GetIp(IHeaderDictionary headers, IPAddress? deafultIp)
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

    public static Encoding? GetHttpRequestEncoding(HttpRequest httpRequest)
    {
        if (httpRequest.Body != null)
        {
            var contentType = httpRequest.ContentType;
            if (!string.IsNullOrEmpty(contentType)
                && MediaTypeHeaderValue.TryParse(contentType, out var attr)
                && !string.IsNullOrEmpty(attr.CharSet))
                return Encoding.GetEncoding(attr.CharSet);
        }

        return null;
    }

    public static Encoding? GetHttpRequestMessageEncoding(HttpRequestMessage httpRequest)
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

    public static async Task SetActivityBody(Activity activity, Stream inputSteam, Encoding? encoding = null)
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
    public static void SetUserInfo(Activity activity, ClaimsPrincipal? claims)
    {
        if (claims == null || !claims.Claims.Any()) return;
        string userId = claims.FindFirstValue(IdentityClaimConsts.USER_ID) ?? claims.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        string userName = claims.FindFirstValue(IdentityClaimConsts.USER_NAME) ?? claims.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

        activity.AddTag(OpenTelemetryAttributeName.EndUser.ID, userId);
        activity.AddTag(OpenTelemetryAttributeName.EndUser.USER_NICK_NAME, userName);
    }

    public static ActivityStatusCode GetStatusResult(int statusCode)
    {
        if (statusCode - 200 >= 0 && statusCode - 299 <= 0 || statusCode - 599 == 0)
            return ActivityStatusCode.Ok;
        if (statusCode - HttpStatusCode.BadRequest >= 0 && statusCode - 599 < 0)
            return ActivityStatusCode.Error;

        return ActivityStatusCode.Unset;
    }

    public virtual void OnException(Activity activity, Exception exception)
    {
        if (exception != null)
        {
            activity.SetTag(OpenTelemetryAttributeName.ExceptionAttributeName.MESSAGE, exception.Message);
            activity.SetTag(OpenTelemetryAttributeName.ExceptionAttributeName.STACKTRACE, exception.ToString());
        }
    }

    public virtual bool IsRecordException { get; set; } = true;
}
