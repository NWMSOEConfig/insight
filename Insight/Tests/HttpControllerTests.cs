namespace Tests;
using NUnit.Framework;

public class HttpControllerTests
{

    HttpController httpController = new HttpController();

    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void TestValidPopulateUrl()
    {
        Assert.IsFalse(httpController.ValidPopulateUrl("isThisAURL?"));
        Assert.IsFalse(httpController.ValidPopulateUrl("https://notaurl/"));
        Assert.IsFalse(httpController.ValidPopulateUrl("https://.newworldnow.com/v7/"));
        Assert.IsFalse(httpController.ValidPopulateUrl("https/applicationsettings"));
        Assert.IsTrue(httpController.ValidPopulateUrl("https://pauat.newworldnow.com/v7/api/applicationsettings/"));    
    }
}