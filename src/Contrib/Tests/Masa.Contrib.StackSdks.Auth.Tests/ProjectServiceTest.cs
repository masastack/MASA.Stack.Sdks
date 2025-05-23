// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Auth.Tests;

[TestClass]
public class ProjectServiceTest
{
    private readonly Mock<HttpMessageHandler> _mockHandler = new();
    private readonly Mock<IHttpClientFactory> _httpClientFactory = new();
    private const string HOST = "http://localhost";
    private const string HTTP_CLIENT_NAME = "masa.contrib.stacksdks.auth";
    private IAuthClient _client = default!;

    [TestInitialize]
    public void Initialized()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton(_httpClientFactory.Object);
        var httpClient = new HttpClient(_mockHandler.Object)
        {
            BaseAddress = new Uri(HOST)
        };
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", new StackSdksBase("auth").UserAgent);
        _httpClientFactory.Setup(factory => factory.CreateClient(HTTP_CLIENT_NAME)).Returns(httpClient);
        services.AddCaller(HTTP_CLIENT_NAME, builder =>
        {
            builder.UseHttpClient(options => options.BaseAddress = HOST);
        });
        var factory = services.BuildServiceProvider().GetRequiredService<ICallerFactory>();
        _client = new AuthClient(factory.Create(HTTP_CLIENT_NAME), new Mock<IMultiEnvironmentUserContext>().Object!, new Mock<IMultilevelCacheClient>().Object!);
    }

    [TestMethod]
    public async Task TestGetGlobalNavigationsAsync()
    {
        var data = new List<ProjectModel>()
        {
            new ProjectModel()
        };
        var userId = Guid.Parse("A9C8E0DD-1E9C-474D-8FE7-8BA9672D53D1");
        _mockHandler.Protected()
           .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
           .ReturnsAsync(new HttpResponseMessage()
           {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(JsonSerializer.Serialize(data))
           }).Verifiable();
        var userContext = new Mock<IMultiEnvironmentUserContext>();
        userContext.Setup(user => user.GetUserId<Guid>()).Returns(userId).Verifiable();
        userContext.SetupGet(user => user.Environment).Returns("development");
        var clientId = "auth-web-dev";
        var result = await _client.ProjectService.GetGlobalNavigations(clientId);
        Assert.IsTrue(result.Count == 1);
    }
}
