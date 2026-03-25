// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Sapp.Tests;

[TestClass]
public class GlobalNavServiceTest
{
    [TestMethod]
    [DataRow("MASA_STACK")]
    public async Task TestGetGlobalNavigationsByClientIdAsync(string clientId)
    {
        var data = new List<GlobalNavigationAppDto> { new() };

        var requestUri = $"api/global-nav/by-client-id/{clientId}";
        var caller = new Mock<ICaller>();
        caller.Setup(provider => provider.GetAsync<List<GlobalNavigationAppDto>>(requestUri, default)).ReturnsAsync(data).Verifiable();
        var sappClient = new SappClient(caller.Object);

        var result = await sappClient.GlobalNavService.GetGlobalNavigationsByClientIdAsync(clientId);
        caller.Verify(provider => provider.GetAsync<List<GlobalNavigationAppDto>>(requestUri, default), Times.Once);

        Assert.IsTrue(result.Count == 1);
    }

    [TestMethod]
    [DataRow("MASA_STACK")]
    public async Task TestGetGlobalNavigationsByClientIdWhenNullAsync(string clientId)
    {
        List<GlobalNavigationAppDto>? data = null;

        var requestUri = $"api/global-nav/by-client-id/{clientId}";
        var caller = new Mock<ICaller>();
        caller.Setup(provider => provider.GetAsync<List<GlobalNavigationAppDto>>(It.IsAny<string>(), default)).ReturnsAsync(data).Verifiable();
        var sappClient = new SappClient(caller.Object);

        var result = await sappClient.GlobalNavService.GetGlobalNavigationsByClientIdAsync(clientId);
        caller.Verify(provider => provider.GetAsync<List<GlobalNavigationAppDto>>(requestUri, default), Times.Once);

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    [DataRow("MASA_STACK")]
    public async Task TestGetVisibleAppEntriesByClientIdAsync(string clientId)
    {
        var data = new List<AppEntryDto> { new() };

        var requestUri = $"api/global-nav/app-entries/by-client-id/{clientId}";
        var caller = new Mock<ICaller>();
        caller.Setup(provider => provider.GetAsync<List<AppEntryDto>>(requestUri, default)).ReturnsAsync(data).Verifiable();
        var sappClient = new SappClient(caller.Object);

        var result = await sappClient.GlobalNavService.GetVisibleAppEntriesByClientIdAsync(clientId);
        caller.Verify(provider => provider.GetAsync<List<AppEntryDto>>(requestUri, default), Times.Once);

        Assert.IsTrue(result.Count == 1);
    }

    [TestMethod]
    [DataRow("MASA_STACK")]
    public async Task TestGetVisibleAppEntriesByClientIdWhenNullAsync(string clientId)
    {
        List<AppEntryDto>? data = null;

        var requestUri = $"api/global-nav/app-entries/by-client-id/{clientId}";
        var caller = new Mock<ICaller>();
        caller.Setup(provider => provider.GetAsync<List<AppEntryDto>>(It.IsAny<string>(), default)).ReturnsAsync(data).Verifiable();
        var sappClient = new SappClient(caller.Object);

        var result = await sappClient.GlobalNavService.GetVisibleAppEntriesByClientIdAsync(clientId);
        caller.Verify(provider => provider.GetAsync<List<AppEntryDto>>(requestUri, default), Times.Once);

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    [DataRow("pm.identity")]
    public async Task TestGetMenusByPmIdentityAsync(string pmIdentity)
    {
        var data = new List<GlobalNavigationNodeDto> { new() };

        var requestUri = $"api/global-nav/menus/by-pm-identity/{pmIdentity}";
        var caller = new Mock<ICaller>();
        caller.Setup(provider => provider.GetAsync<List<GlobalNavigationNodeDto>>(requestUri, default)).ReturnsAsync(data).Verifiable();
        var sappClient = new SappClient(caller.Object);

        var result = await sappClient.GlobalNavService.GetMenusByPmIdentityAsync(pmIdentity);
        caller.Verify(provider => provider.GetAsync<List<GlobalNavigationNodeDto>>(requestUri, default), Times.Once);

        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    [DataRow("pm.identity")]
    public async Task TestGetMenusByPmIdentityWhenNullAsync(string pmIdentity)
    {
        List<GlobalNavigationNodeDto>? data = null;

        var requestUri = $"api/global-nav/menus/by-pm-identity/{pmIdentity}";
        var caller = new Mock<ICaller>();
        caller.Setup(provider => provider.GetAsync<List<GlobalNavigationNodeDto>>(It.IsAny<string>(), default)).ReturnsAsync(data).Verifiable();
        var sappClient = new SappClient(caller.Object);

        var result = await sappClient.GlobalNavService.GetMenusByPmIdentityAsync(pmIdentity);
        caller.Verify(provider => provider.GetAsync<List<GlobalNavigationNodeDto>>(requestUri, default), Times.Once);

        Assert.AreEqual(0, result.Count);
    }
}
