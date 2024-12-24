// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Tsc.OpenTelemetry;

internal class HttpResponseMiddleware
{
    private readonly RequestDelegate _next;

    public HttpResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var httpResponse = httpContext.Response;
        using var ms = new MemoryStream();
        var rawStream = httpResponse.Body;
        httpResponse.Body = ms;
        await _next(httpContext);
        ms.Seek(0, SeekOrigin.Begin);
        var responseResult = await new StreamReader(ms).ReadToEndAsync();
        ms.Seek(0, SeekOrigin.Begin);
        await ms.CopyToAsync(rawStream);
        httpResponse.Body = rawStream;

        if (httpResponse.StatusCode - 299 == 0 || httpResponse.StatusCode - 500 >= 0)
        {
            Activity.Current?.SetTag(OpenTelemetryAttributeName.Http.RESPONSE_CONTENT_BODY, responseResult);
        }
        else
        {
            OpenTelemetryInstrumentationOptions.Logger?.LogInformation("response length: {Length}, context: {Context}", responseResult.Length, responseResult);
        }
    }
}
