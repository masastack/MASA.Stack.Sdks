// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Tsc.OpenTelemetry.Tests.Trace;

[TestClass]
public class ActivityTest
{
    [TestInitialize]
    public void Initialize()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    [TestMethod]
    public void HttpRequestMessageAddTagsTest()
    {
        HttpRequestMessage request = new()
        {
            Method = HttpMethod.Post
        };
        var body = "{\"name\":\"张三\"}";
        request.Content = new StringContent(body, Encoding.GetEncoding("gbk"), "application/json");
        request.RequestUri = new Uri("http://localhost");

        var activity = new Activity("tets");
        HttpClientInstrumentHandler.AddMasaHttpRequestMessage(activity, request);
        Assert.AreEqual(body, activity.GetTagItem(OpenTelemetryAttributeName.Http.REQUEST_CONTENT_BODY) as string);
    }

    [TestMethod]
    public void HttpResponseMessageAddTagsTest()
    {
        HttpResponseMessage response = new()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("OK")
        };

        var activity = new Activity("tets");
        HttpClientInstrumentHandler.AddMasaHttpResponseMessage(activity, response);
        Assert.IsNotNull(activity);
    }

    [TestMethod]
    public void HttpRequestMessageAddMultipartFormTagsTest()
    {
        HttpRequestMessage request = new()
        {
            Method = HttpMethod.Post
        };

        var multipartContent = new MultipartFormDataContent("----masa-boundary");
        multipartContent.Add(new StringContent("demo-name"), "name");
        multipartContent.Add(new ByteArrayContent(Encoding.UTF8.GetBytes("file-content")), "uploadFile", "demo.txt");
        request.Content = multipartContent;
        request.RequestUri = new Uri("http://localhost");

        var activity = new Activity("tets");
        HttpClientInstrumentHandler.AddMasaHttpRequestMessage(activity, request);
        var body = activity.GetTagItem(OpenTelemetryAttributeName.Http.REQUEST_CONTENT_BODY) as string;

        Assert.IsNotNull(body);
        Assert.IsTrue(body.Contains("name=demo-name"));
        Assert.IsTrue(body.Contains("uploadFile=[file:demo.txt"));
    }

    [TestMethod]
    public void HttpRequestAddTagsTest()
    {
        Mock<HttpRequest> mock = new();
        mock.Setup(request => request.Method).Returns("post");
        mock.Setup(request => request.ContentType).Returns(" application/json; charset=gbk");
        mock.Setup(request => request.Host).Returns(new HostString("http://localhost"));
        mock.Setup(request => request.Protocol).Returns("http1.1");
        mock.Setup(request => request.Scheme).Returns("http");
        var httpContext = new Mock<HttpContext>();
        mock.Setup(request => request.HttpContext).Returns(httpContext.Object);

        var body = "{\"name\":\"张三\"}";
        var bytes = Encoding.GetEncoding("GBK").GetBytes(body);
        using var ms = new MemoryStream(bytes);
        mock.Setup(request => request.Body).Returns(ms);
        mock.Setup(request => request.ContentLength).Returns(ms.Length);

        var activity = new Activity("tets");
        AspNetCoreInstrumentationHandler.AddMasaHttpRequest(activity, mock.Object);
        Assert.IsNotNull(activity);
        Assert.AreEqual("http", activity.GetTagItem(OpenTelemetryAttributeName.Http.SCHEME) as string);
        Assert.AreEqual("http1.1", activity.GetTagItem(OpenTelemetryAttributeName.Http.FLAVOR) as string);
        Assert.AreEqual(body, activity.GetTagItem(OpenTelemetryAttributeName.Http.REQUEST_CONTENT_BODY) as string);
    }

    [TestMethod]
    public void HttpResponseAddTagsTest()
    {
        Mock<HttpResponse> mock = new();
        mock.Setup(request => request.StatusCode).Returns(200);
        mock.Setup(request => request.ContentType).Returns(" application/json; charset=gbk");

        var httpContext = new Mock<HttpContext>();
        httpContext.Setup(context => context.User).Returns(new ClaimsPrincipal(new List<ClaimsIdentity> {
        new ClaimsIdentity( new Claim[]{ new(IdentityClaimConsts.USER_ID,"123456") },"userId"),
         new ClaimsIdentity( new Claim[]{ new(IdentityClaimConsts.USER_ID, "admin") },"userName")
    }));
        mock.Setup(request => request.HttpContext).Returns(httpContext.Object);
        var body = "{\"name\":\"张三\"}";
        var bytes = Encoding.GetEncoding("GBK").GetBytes(body);
        using var ms = new MemoryStream(bytes);
        mock.Setup(request => request.Body).Returns(ms);
        mock.Setup(request => request.ContentLength).Returns(ms.Length);

        var activity = new Activity("tets");
        AspNetCoreInstrumentationHandler.AddMasaHttpResponse(activity, mock.Object);
        Assert.IsNotNull(activity.GetTagItem(OpenTelemetryAttributeName.Http.RESPONSE_CONTENT_TYPE) as string);
    }

    [TestMethod]
    public void HttpRequestAddMultipartFormTagsTest()
    {
        var fileStream = new MemoryStream(Encoding.UTF8.GetBytes("file-content"));
        var formFile = new FormFile(fileStream, 0, fileStream.Length, "uploadFile", "demo.txt")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain"
        };

        var formCollection = new FormCollection(
            new Dictionary<string, Microsoft.Extensions.Primitives.StringValues> { { "name", "demo-name" } },
            new FormFileCollection { formFile });

        var httpContext = new Mock<HttpContext>();
        httpContext.Setup(context => context.User).Returns(new ClaimsPrincipal());

        var request = new Mock<HttpRequest>();
        request.Setup(r => r.Protocol).Returns("HTTP/1.1");
        request.Setup(r => r.Scheme).Returns("http");
        request.Setup(r => r.ContentLength).Returns(0);
        request.Setup(r => r.ContentType).Returns("multipart/form-data; boundary=----masa-boundary");
        request.Setup(r => r.HttpContext).Returns(httpContext.Object);
        request.Setup(r => r.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes("unused")));
        request.Setup(r => r.ReadFormAsync(It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(formCollection);

        var activity = new Activity("tets");
        AspNetCoreInstrumentationHandler.AddMasaHttpRequest(activity, request.Object);
        var body = activity.GetTagItem(OpenTelemetryAttributeName.Http.REQUEST_CONTENT_BODY) as string;

        Assert.IsNotNull(body);
        Assert.IsTrue(body.Contains("name=demo-name"));
        Assert.IsTrue(body.Contains("uploadFile=[file:demo.txt"));
    }
}
