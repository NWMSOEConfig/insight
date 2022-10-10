using InsightApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace InsightApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    private static readonly IList<Parameter> _parameters = new List<Parameter>
    {
        new(0, "Enabled", "boolean", true),
        new(1, "Foo", "number", 123),
        new(2, "Bar", "text", "Text"),
        new(3, "Baz", "email", "a@b.com"),
    };

    private static readonly IList<Setting> _settings = new List<Setting>
    {
        new(0, "Foo", new List<int> { 0, 1 }),
        new(1, "Bar", new List<int> { 2 }),
        new(2, "Baz", new List<int> { 3 }),
    };

    [HttpGet]
    [Route("parameter")]
    public IActionResult GetParameter(int id)
    {
        return id < 0 || id >= _parameters.Count ? BadRequest() : Ok(_parameters[id]);
    }

    [HttpGet]
    [Route("setting")]
    public IActionResult GetSetting(int id)
    {
        return id < 0 || id >= _settings.Count ? BadRequest() : Ok(_settings[id]);
    }
}
