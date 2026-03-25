// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Sapp.Tests;

[TestClass]
public class SappClientTest
{
    [TestMethod]
    public void TestAddSappClient()
    {
        var services = new ServiceCollection();

        services.AddSappClient(option =>
        {
            option.UseHttpClient(builder =>
            {
                builder.Configure = opt => opt.BaseAddress = new Uri("https://github.com");
            });
        });

        var sappClient = services.BuildServiceProvider().GetRequiredService<ISappClient>();
        Assert.IsNotNull(sappClient);
    }

    [TestMethod]
    public void TestAddSappClientByAddress()
    {
        var services = new ServiceCollection();

        services.AddSappClient("https://github.com");

        var sappClient = services.BuildServiceProvider().GetRequiredService<ISappClient>();
        Assert.IsNotNull(sappClient);
    }

    [TestMethod]
    public void TestAddSappClientShouldThrowArgumentNullException()
    {
        var services = new ServiceCollection();

        Assert.ThrowsException<MasaArgumentException>(() => services.AddSappClient(""));
    }

    [TestMethod]
    public void TestAddMultipleSappClient()
    {
        var services = new ServiceCollection();

        services.AddSappClient(option =>
        {
            option.UseHttpClient(builder =>
            {
                builder.Configure = opt => opt.BaseAddress = new Uri("https://github.com");
            });
        });

        services.AddSappClient(option =>
        {
            option.UseHttpClient(builder =>
            {
                builder.Configure = opt => opt.BaseAddress = new Uri("https://github.com");
            });
        });

        var sappClient = services.BuildServiceProvider().GetRequiredService<ISappClient>();
        Assert.IsNotNull(sappClient);
    }
}
