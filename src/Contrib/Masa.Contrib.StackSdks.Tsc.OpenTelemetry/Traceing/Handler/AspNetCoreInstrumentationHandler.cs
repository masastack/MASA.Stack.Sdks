// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

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
            SetMasaCustomerHeaderTags(activity, httpRequest.Headers);
        }

        SetUserInfo(activity, httpRequest.HttpContext.User);
        if (httpRequest.Body != null)
        {
            if (TrySetMultipartFormBody(activity, httpRequest))
            {
                activity.SetTag(OpenTelemetryAttributeName.Host.NAME, Dns.GetHostName());
                return;
            }

            if (!httpRequest.Body.CanSeek)
                httpRequest.EnableBuffering();
            SetActivityBody(activity, httpRequest.Body, GetHttpRequestEncoding(httpRequest));
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

    private static void SetMasaCustomerHeaderTags(Activity activity, IHeaderDictionary headers)
    {
        if (OpenTelemetryInstrumentationOptions.MasaCustomerHeaders == null || OpenTelemetryInstrumentationOptions.MasaCustomerHeaders.Length == 0 || headers == null || headers.Count == 0)
            return;
        foreach (var header in OpenTelemetryInstrumentationOptions.MasaCustomerHeaders)
        {
            if (string.IsNullOrEmpty(header)) continue;
            var headerValue = GetHeaderValue(headers, header);
            if (headerValue != null)
            {
                activity.SetTag($"{header.ToLower().Replace('-', '.')}", headerValue?.ToString());
            }
        }
    }

    private static object GetHeaderValue(IHeaderDictionary headers, string key)
    {
        return headers.TryGetValue(key, out StringValues value) ? value : default;
    }

    private static bool TrySetMultipartFormBody(Activity activity, HttpRequest httpRequest)
    {
        if (!IsMultipartFormData(httpRequest.ContentType))
            return false;

        if (!httpRequest.Body.CanSeek)
            httpRequest.EnableBuffering();

        var form = httpRequest.ReadFormAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        var formItems = new List<string>();
        foreach (var formValue in form)
        {
            formItems.Add($"{formValue.Key}={formValue.Value}");
        }

        foreach (var formFile in form.Files)
        {
            var stream = formFile.OpenReadStream();
            LogFormFileContent("AspNetCore", formFile.Name, formFile.FileName, stream);
            formItems.Add(GetFileFormValue(formFile.Name, formFile.FileName, formFile.ContentType, formFile.Length));
        }

        if (httpRequest.Body.CanSeek)
            httpRequest.Body.Seek(0, SeekOrigin.Begin);

        if (formItems.Count > 0)
            activity.SetTag(OpenTelemetryAttributeName.Http.REQUEST_CONTENT_BODY, string.Join("&", formItems));

        return true;
    }

    private static bool IsMultipartFormData(string? contentType)
    {
        return !string.IsNullOrWhiteSpace(contentType) && contentType.StartsWith("multipart/form-data", StringComparison.OrdinalIgnoreCase);
    }

    private static string GetFileFormValue(string key, string fileName, string? contentType, long? fileLength)
    {
        var fileInfo = $"[file:{fileName}";
        if (!string.IsNullOrWhiteSpace(contentType))
            fileInfo += $",contentType:{contentType}";
        if (fileLength.HasValue && fileLength > 0)
            fileInfo += $",length:{fileLength.Value}";
        fileInfo += "]";
        return $"{key}={fileInfo}";
    }

    private static void LogFormFileContent(string source, string key, string fileName, Stream fileStream)
    {
        (long length, string? content) = fileStream.ReadAsBase64();
        if (length <= 0)
            return;
        if (length - OpenTelemetryInstrumentationOptions.MaxBodySize > 0)
        {
            OpenTelemetryInstrumentationOptions.Logger?.LogInformation("[{Source}] keyName: {Key}, fileName: {FileName}, length: {Length}, max: {MaxBodySize}", source, key, fileName, length, OpenTelemetryInstrumentationOptions.MaxBodySize);
            return;
        }

        OpenTelemetryInstrumentationOptions.Logger?.LogInformation("[{Source}] keyName: {Key}, fileName: {FileName}, base64 content: {Content}", source, key, fileName, content ?? string.Empty);
    }
}
