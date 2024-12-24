// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Http;

namespace Masa.Contrib.StackSdks.Tsc.OpenTelemetry.Tracing.Handler;

internal class AspNetCoreInstrumentationHandler : ExceptionHandler
{
    public virtual void OnHttpRequest(Activity activity, HttpRequest httpRequest)
    {
        AddMasaHttpRequest(activity, httpRequest);
        AddBlazorServerRoute(activity, httpRequest);
        HttpMetricProviders.AddHttpRequestMetric(httpRequest);
    }

    public virtual void OnHttpResponse(Activity activity, HttpResponse httpResponse)
    {
        AddMasaHttpResponse(activity, httpResponse);

        HttpMetricProviders.AddHttpResponseMetric(httpResponse);
    }

    public static void AddMasaHttpRequest(Activity activity, HttpRequest httpRequest)
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
    }

    public static void AddMasaHttpResponse(Activity activity, HttpResponse httpResponse)
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
    }

    public static void AddBlazorServerRoute(Activity activity, HttpRequest httpRequest)
    {
        if (!BlazorFilterExtenistion.IsBlazorFilter(httpRequest.HttpContext))
            return;
        if (BlazorRouteManager.EnableBlazorRoute && BlazorRouteManager.TryGetUrlRoute(httpRequest.Path, out var route))
        {
            activity?.SetTag("http.route", route!.Template);
            activity?.SetTag("blazor.server.type", route!.Type);
        }
    }
}
