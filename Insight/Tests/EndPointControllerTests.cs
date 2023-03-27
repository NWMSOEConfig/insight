namespace Tests;

using Insight.Controllers;
using Insight.Models;
using Insight.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

public class EndPointControllerTests
{
    const string URL = "https://pauat.newworldnow.com/v7/api/applicationsettings/";

    [Test]
    public async Task TestGetQueuedSettingNullQueue()
    {
        var mockService = new Mock<DatabaseQueuedChangeService>(null);
        mockService.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((QueuedChange?)null);
        var database = new DataServer(null, null, null, mockService.Object, null, null);
        var controller = new DataController(database);
        Assert.AreEqual(null, await controller.GetQueuedSetting("a", "b", "c", "d"));
    }

    [Test]
    public async Task TestGetQueuedSettingNullSetting()
    {
        var mockService = new Mock<DatabaseQueuedChangeService>(null);
        mockService.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new QueuedChange
        {
            Settings = new List<ChangedSetting>(),
        });
        var database = new DataServer(null, null, null, mockService.Object, null, null);
        var controller = new DataController(database);
        Assert.AreEqual(null, await controller.GetQueuedSetting("a", "b", "c", "d"));
    }

    [Test]
    public async Task TestGetQueuedSetting()
    {
        var mockService = new Mock<DatabaseQueuedChangeService>(null);
        mockService.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new QueuedChange
        {
            Settings = new List<ChangedSetting>
            {
                new ChangedSetting {
                oldSetting = null!,
                newSetting = new DatabaseSetting
                    {
                        Name = "Foo",
                        Parameters = new Parameter[]
                        {
                            new Parameter("Bar"),
                        },
                    },
            }
            },
        });
        var database = new DataServer(null, null, null, mockService.Object, null, null);
        var controller = new DataController(database);
        var setting = await controller.GetQueuedSetting("Foo", "b", "c", "d");
        Assert.AreEqual("Foo", setting.Name);
        Assert.AreEqual(null, setting.Category);
        Assert.AreEqual(null, setting.Tenant);
        Assert.AreEqual(1, setting.Parameters.Count);
        Assert.AreEqual("Bar", setting.Parameters[0].Value);
    }

    [Test]
    public async Task TestGetSettingAsyncNullSetting()
    {
        var mockService = new Mock<DatabaseQueuedChangeService>(null);
        var mockTenant = new Mock<DatabaseTenantService>(null);
        mockService.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((QueuedChange?)null);
        mockTenant.Setup(x => x.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(
            new DatabaseTenant
            {
                Environments = new DatabaseEnvironment[]
                {
                    new DatabaseEnvironment
                    {
                        Name = "c",
                        Url = URL,
                    }
                }
            }
        );
        var database = new DataServer(null, mockTenant.Object, null, mockService.Object, null, null);
        var controller = new DataController(database);
        Assert.IsInstanceOf<BadRequestResult>(await controller.GetSettingAsync("a", "b", "c"));
    }

    [Test]
    public async Task TestGetSettingAsync()
    {
        var mockService = new Mock<DatabaseQueuedChangeService>(null);
        var mockTenant = new Mock<DatabaseTenantService>(null);
        mockService.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new QueuedChange
        {
            Settings = new List<ChangedSetting>
            {new ChangedSetting {
                oldSetting = null!,
                newSetting = new DatabaseSetting
                    {
                        Name = "Foo",
                        Parameters = new Parameter[]
                        {
                            new Parameter("Bar"),
                        },
                    },
            }
            },
        });
        mockTenant.Setup(x => x.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(
            new DatabaseTenant
            {
                Environments = new DatabaseEnvironment[]
                {
                    new DatabaseEnvironment
                    {
                        Name = "c",
                        Url = URL,
                    }
                }
            }
        );
        var database = new DataServer(null, mockTenant.Object, null, mockService.Object, null, null);
        var controller = new DataController(database);
        Assert.IsInstanceOf<OkObjectResult>(await controller.GetSettingAsync("Foo", "b", "c"));
    }
}
