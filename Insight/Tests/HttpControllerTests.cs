namespace Tests;

using System.Net;
using Moq;
using Moq.Protected;
using NUnit.Framework;

public class HttpControllerTests
{
    const string URL = "https://pauat.newworldnow.com/v7/api/applicationsettings/";
    HttpController httpController = new HttpController();

    [Test]
    public void TestValidPopulateUrl()
    {
        Assert.IsFalse(httpController.ValidPopulateUrl("isThisAURL?"));
        Assert.IsFalse(httpController.ValidPopulateUrl("https://notaurl/"));
        Assert.IsFalse(httpController.ValidPopulateUrl("https://.newworldnow.com/v7/"));
        Assert.IsFalse(httpController.ValidPopulateUrl("https/applicationsettings"));
        Assert.IsTrue(httpController.ValidPopulateUrl("https://pauat.newworldnow.com/v7/api/applicationsettings/"));    
    }

    [Test]
    public void TestPopulateGetRequestRequiresSuccessCode()
    {
        SetupRequestOutput(string.Empty, HttpStatusCode.InternalServerError);
        Assert.ThrowsAsync<HttpRequestException>(async () => await httpController.PopulateGetRequest(URL));
    }

    [Test]
    public void TestPopulateGetRequestRequiresValidUrl()
    {
        SetupRequestOutput(string.Empty);
        Assert.ThrowsAsync<ArgumentException>(async () => await httpController.PopulateGetRequest("abc"));
    }

    [Test]
    public void TestPopulateGetRequestRequiresValidResponse()
    {
        SetupRequestOutput("null");
        Assert.ThrowsAsync<NullReferenceException>(async () => await httpController.PopulateGetRequest(URL));
    }

    [Test]
    public async Task TestPopulateGetRequestReturnsData()
    {
        SetupRequestOutput(@"[{""Name"":""Foo"",""Value"":""Test""}]");
        var result = await httpController.PopulateGetRequest(URL);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("Foo", result[0].Name);
        Assert.AreEqual("Test", result[0].Parameters[0].Value);
    }

    private void SetupRequestOutput(string response, HttpStatusCode status = HttpStatusCode.OK)
    {
        // based on https://stackoverflow.com/a/44028625
        var mock = new Mock<HttpMessageHandler>();
        mock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = status,
                Content = new StringContent(response),
            });
        httpController = new HttpController(mock.Object);
    }
}