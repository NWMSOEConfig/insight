namespace Tests;
using NUnit.Framework;
using Insight.Services;

public class CommitServiceTests
{

    DatabaseCommitService service = new DatabaseCommitService();

    [SetUp]
    public void Setup()
    {
        service = new DatabaseCommitService();
    }

    [Test]
    public void TestConstructor()
    {
        Assert.IsNotNull(service);
    }

    [Test]
    public void TestGet()
    {
        Assert.NotNull(service.GetAsync("0")); // will fail
    }

     [Test]
    public void TestGetTime()
    {
           
    }

    [Test]
    public void TestUpdate()
    {
           
    }

    [Test]
    public void TestRemove()
    {
           
    }
}