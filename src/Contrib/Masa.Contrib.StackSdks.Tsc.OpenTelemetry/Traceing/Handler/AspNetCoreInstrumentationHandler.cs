// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Tsc.OpenTelemetry.Tracing.Handler;

public class AspNetCoreInstrumentationHandler : ExceptionHandler
{
    public virtual void OnHttpRequest(Activity activity, HttpRequest httpRequest)
    {
        activity.AddMasaHttpRequest(httpRequest);
        HttpMetricProviders.AddHttpRequestMetric(httpRequest);
    }

    public virtual void OnHttpResponse(Activity activity, HttpResponse httpResponse)
    {
        activity.AddMasaHttpResponse(httpResponse);
        HttpMetricProviders.AddHttpResponseMetric(httpResponse);
    }
}
