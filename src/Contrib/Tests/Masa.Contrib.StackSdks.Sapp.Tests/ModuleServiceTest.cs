// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Sapp.Tests;

[TestClass]
public class ModuleServiceTest
{
    [TestMethod]
    [DataRow("pm.identity")]
    public async Task TestGetByPmIdentityAsync(string pmIdentity)
    {
        var data = new ModuleDetailDto { PmIdentity = pmIdentity };

        var requestUri = $"api/module/byPmIdentity/{pmIdentity}";
        var caller = new Mock<ICaller>();
        caller.Setup(provider => provider.GetAsync<ModuleDetailDto?>(requestUri, default)).ReturnsAsync(data).Verifiable();
        var sappClient = new SappClient(caller.Object);

        var result = await sappClient.ModuleService.GetByPmIdentityAsync(pmIdentity);
        caller.Verify(provider => provider.GetAsync<ModuleDetailDto?>(requestUri, default), Times.Once);

        Assert.IsNotNull(result);
        Assert.AreEqual(pmIdentity, result.PmIdentity);
    }

    [TestMethod]
    [DataRow("pm.identity")]
    public async Task TestGetByPmIdentityWhenNullAsync(string pmIdentity)
    {
        ModuleDetailDto? data = null;

        var requestUri = $"api/module/byPmIdentity/{pmIdentity}";
        var caller = new Mock<ICaller>();
        caller.Setup(provider => provider.GetAsync<ModuleDetailDto?>(It.IsAny<string>(), default)).ReturnsAsync(data).Verifiable();
        var sappClient = new SappClient(caller.Object);

        var result = await sappClient.ModuleService.GetByPmIdentityAsync(pmIdentity);
        caller.Verify(provider => provider.GetAsync<ModuleDetailDto?>(requestUri, default), Times.Once);

        Assert.IsNull(result);
    }
}
