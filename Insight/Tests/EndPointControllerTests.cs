namespace Tests;

using Insight.Controllers;
using Insight.Models;
using Insight.Services;
using Moq;
using NUnit.Framework;

public class EndPointControllerTests
{
    [Test]
    public async Task TestGetQueuedSettingNullQueue()
    {
        var mockService = new Mock<DatabaseQueuedChangeService>();
        mockService.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((QueuedChange?)null);
        var database = new DataServer(null, null, null, mockService.Object, null, null);
        var controller = new DataController(database);
        Assert.AreEqual(null, await controller.GetQueuedSetting("a", "b", "c", "d"));
    }

    [Test]
    public async Task TestGetQueuedSettingNullSetting()
    {
        var mockService = new Mock<DatabaseQueuedChangeService>();
        mockService.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new QueuedChange
        {
            Settings = new DatabaseSetting[] { },
        });
        var database = new DataServer(null, null, null, mockService.Object, null, null);
        var controller = new DataController(database);
        Assert.AreEqual(null, await controller.GetQueuedSetting("a", "b", "c", "d"));
    }

    [Test]
    public async Task TestGetQueuedSetting()
    {
        var mockService = new Mock<DatabaseQueuedChangeService>();
        mockService.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new QueuedChange
        {
            Settings = new DatabaseSetting[]
            {
                new DatabaseSetting
                {
                    Name = "Foo",
                    Parameters = new Parameter[]
                    {
                        new Parameter("Bar"),
                    },
                },
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
}
