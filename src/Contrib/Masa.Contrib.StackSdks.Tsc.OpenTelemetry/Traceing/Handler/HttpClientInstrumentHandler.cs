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
            SetMasaCustomerHeaderTags(activity, httpRequest.Headers);
        }
        if (httpRequest.Content != null)
        {
            if (TrySetMultipartFormBody(activity, httpRequest))
                return;

            var stream = httpRequest.Content.ReadAsStreamAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            SetActivityBody(activity, stream, GetHttpRequestMessageEncoding(httpRequest));
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

    private static void SetMasaCustomerHeaderTags(Activity activity, HttpRequestHeaders headers)
    {
        if (OpenTelemetryInstrumentationOptions.MasaCustomerHeaders == null || OpenTelemetryInstrumentationOptions.MasaCustomerHeaders.Length == 0 || headers == null || !headers.Any())
            return;

        foreach (var header in OpenTelemetryInstrumentationOptions.MasaCustomerHeaders)
        {
            if (string.IsNullOrEmpty(header)) continue;
            if (headers.TryGetValues(header, out var headerValue) && headerValue != null)
            {
                activity.SetTag($"{header.ToLower().Replace('-', '.')}", headerValue?.ToString());
            }
        }
    }

    private static bool TrySetMultipartFormBody(Activity activity, HttpRequestMessage httpRequest)
    {
        if (!IsMultipartFormData(httpRequest.Content?.Headers?.ContentType?.MediaType) || httpRequest.Content is not MultipartFormDataContent formDataContent)
            return false;

        var formItems = new List<string>();
        foreach (var item in formDataContent)
        {
            var key = TrimQuotes(item.Headers.ContentDisposition?.Name);
            if (string.IsNullOrWhiteSpace(key))
                continue;

            var fileName = TrimQuotes(item.Headers.ContentDisposition?.FileNameStar ?? item.Headers.ContentDisposition?.FileName);
            if (!string.IsNullOrEmpty(fileName))
            {
                var fileStream = item.ReadAsStreamAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                LogFormFileContent("HttpClient", key, fileName, fileStream);
                formItems.Add(GetFileFormValue(key, fileName, item.Headers.ContentType?.MediaType, GetStreamLength(fileStream)));
                continue;
            }

            var contentStream = item.ReadAsStreamAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            (_, var value) = contentStream.ReadAsStringAsync(GetHttpRequestMessageEncoding(httpRequest)).ConfigureAwait(false).GetAwaiter().GetResult();
            formItems.Add($"{key}={value ?? string.Empty}");
        }

        if (formItems.Count > 0)
            activity.SetTag(OpenTelemetryAttributeName.Http.REQUEST_CONTENT_BODY, string.Join("&", formItems));

        return true;
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
        var logger = OpenTelemetryInstrumentationOptions.Logger;
        (long length, string? content) = fileStream.ReadAsBase64();
        if (length <= 0)
            return;

        using var scope = logger?.BeginScope(new Dictionary<string, object?>
        {
            ["source"] = source,
            ["form.key"] = key,
            ["file.name"] = fileName,
            ["file.length"] = length
        });

        if (length - OpenTelemetryInstrumentationOptions.MaxBodySize > 0)
        {
            logger?.LogInformation("file content exceeded max limit. max: {MaxBodySize}", OpenTelemetryInstrumentationOptions.MaxBodySize);
            return;
        }

        logger?.LogInformation("file content(base64): {Content}", content ?? string.Empty);
    }

    private static long? GetStreamLength(Stream stream)
    {
        return stream.CanSeek ? stream.Length : null;
    }

    private static bool IsMultipartFormData(string? mediaType)
    {
        return !string.IsNullOrWhiteSpace(mediaType) && mediaType.StartsWith("multipart/form-data", StringComparison.OrdinalIgnoreCase);
    }

    private static string? TrimQuotes(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        return value.Trim().Trim('"');
    }
}
