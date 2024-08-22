// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Tsc.OpenTelemetry.Tracing.Handler;

internal class HttpClientInstrumentHandler : ExceptionHandler
{
    public virtual void OnHttpRequestMessage(Activity activity, HttpRequestMessage httpRequestMessage)
    {
        AddMasaHttpRequestMessage(activity, httpRequestMessage);
        HttpMetricProviders.AddHttpRequestMessageMetric(httpRequestMessage);
    }

    public virtual void OnHttpResponseMessage(Activity activity, HttpResponseMessage httpResponseMessage)
    {
        AddMasaHttpResponseMessage(activity, httpResponseMessage);
        HttpMetricProviders.AddHttpResponseMessageMetric(httpResponseMessage);
    }

    public static void AddMasaHttpRequestMessage(Activity activity, HttpRequestMessage httpRequest)
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
    }

    public static void AddMasaHttpResponseMessage(Activity activity, HttpResponseMessage httpResponse)
    {
        activity.SetTag(OpenTelemetryAttributeName.Host.NAME, Dns.GetHostName());
        activity.SetStatus(GetStatusResult((int)httpResponse.StatusCode));
    }

    public virtual void OnHttpWebRequest(Activity activity, HttpWebRequest httpWebRequest)
    {

    }

    public virtual void OnHttpWebResponse(Activity activity, HttpWebResponse httpWebResponse)
    {

    }
}
