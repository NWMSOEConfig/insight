namespace Tests;
using NUnit.Framework;
using Insight.Controllers;
using Moq;
using Insight.Models;

public class EndpointTests
{
    UserController userController;

    [SetUp]
    public void Setup()
    {
        var mock = new Mock<IDataServer>();
        userController = new UserController(mock.Object);
        mock.Setup(p => p.GetQueue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns( value: null as Task<QueuedChange?>);


    }

    [Test]
    public async void TestLiveSettingNoQueuedSetting()
    {
       //await userController.GetSettingAsync();
    }
}