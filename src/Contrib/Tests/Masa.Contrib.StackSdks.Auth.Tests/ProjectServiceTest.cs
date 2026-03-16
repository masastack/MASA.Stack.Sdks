// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Auth.Tests;

[TestClass]
public class ProjectServiceTest
{
    [TestMethod]
    public async Task TestGetGlobalNavigationsAsync()
    {
        var userId = Guid.Parse("A9C8E0DD-1E9C-474D-8FE7-8BA9672D53D1");
        var data = new List<ProjectModel> { new() };
        var requestUri = $"api/project/navigations?userId={userId}&clientId=auth-web-dev";
        var caller = new Mock<ICaller>();
        caller.Setup(provider => provider.GetAsync<List<ProjectModel>>(requestUri, default)).ReturnsAsync(data).Verifiable();
        var userContext = new Mock<IUserContext>();
        userContext.Setup(user => user.GetUserId<Guid>()).Returns(userId).Verifiable();
        var service = GetProjectService(caller, userContext);

        var result = await service.GetGlobalNavigations("auth-web-dev");

        caller.Verify(provider => provider.GetAsync<List<ProjectModel>>(requestUri, default), Times.Once);
        userContext.Verify(user => user.GetUserId<Guid>(), Times.Once);
        Assert.IsTrue(result.Count == 1);
    }

    [TestMethod]
    public async Task TestGetGlobalNavigationsByMultiAppIdsAsync()
    {
        var data = new List<ProjectModel> { new() };
        var requestUri = "api/project/byAppIds?appIds=app1%2Capp2";
        var caller = new Mock<ICaller>();
        caller.Setup(provider => provider.GetAsync<List<ProjectModel>>(requestUri, default)).ReturnsAsync(data).Verifiable();
        var service = GetProjectService(caller);

        var result = await service.GetNavigationsByAppId("app1", "app2");

        caller.Verify(provider => provider.GetAsync<List<ProjectModel>>(requestUri, default), Times.Once);
        Assert.IsTrue(result.Count == 1);
    }

    [TestMethod]
    public async Task TestGetGlobalNavigationsBySingleAppIdAsync()
    {
        var data = new List<ProjectModel> { new() };
        var requestUri = "api/project/byAppIds?appIds=auth-web-dev";
        var caller = new Mock<ICaller>();
        caller.Setup(provider => provider.GetAsync<List<ProjectModel>>(requestUri, default)).ReturnsAsync(data).Verifiable();
        var service = GetProjectService(caller);

        var result = await service.GetNavigationsByAppId("auth-web-dev");

        caller.Verify(provider => provider.GetAsync<List<ProjectModel>>(requestUri, default), Times.Once);
        Assert.IsTrue(result.Count == 1);
    }

    [TestMethod]
    public async Task TestGetMenuDetailAsync()
    {
        var menuId = Guid.Parse("225082D3-CC88-48D2-3C27-08DA3ED8F4B7");
        var data = new NavDetailModel { Id = menuId };
        var requestUri = "api/project/menus/detail";
        var caller = new Mock<ICaller>();
        caller.Setup(provider => provider.GetAsync<object, NavDetailModel>(requestUri, It.IsAny<object>(), default)).ReturnsAsync(data).Verifiable();
        var service = GetProjectService(caller);

        var result = await service.GetMenuDetailAsync(menuId);

        caller.Verify(provider => provider.GetAsync<object, NavDetailModel>(requestUri, It.IsAny<object>(), default), Times.Once);
        Assert.IsNotNull(result);
        Assert.AreEqual(menuId, result.Id);
    }

    [TestMethod]
    public async Task TestUpdateMenuAsync()
    {
        var input = new UpdateNavModel
        {
            Id = Guid.Parse("225082D3-CC88-48D2-3C27-08DA3ED8F4B7"),
            Icon = "mdi-home",
            MatchPattern = "/home.*"
        };
        var requestUri = "api/project/menus/meta";
        var caller = new Mock<ICaller>();
        caller.Setup(provider => provider.PutAsync(requestUri, input, true, default)).Verifiable();
        var service = GetProjectService(caller);

        await service.UpdateMenuAsync(input);

        caller.Verify(provider => provider.PutAsync(requestUri, input, true, default), Times.Once);
    }

    private static ProjectService GetProjectService(Mock<ICaller> caller, Mock<IUserContext>? userContext = null)
    {
        return new ProjectService(caller.Object, userContext?.Object ?? new Mock<IUserContext>().Object);
    }
}
