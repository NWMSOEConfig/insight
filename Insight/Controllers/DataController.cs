using Insight.Models;
using Microsoft.AspNetCore.Mvc;

namespace Insight.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    private HttpController httpController = new HttpController();
    private DataServer _dbController;

     public DataController(DataServer databaseController) =>
        _dbController = databaseController;
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

    private static readonly IList<Subcategory> _subcategories = new List<Subcategory>
    {
        new(0, "Subcategory 1", new List<int> { 0 }),
        new(1, "Subcategory 2", new List<int> { 1 }),
        new(2, "Subcategory 3", new List<int> { 2 })
    };

    private static readonly IList<Category> _categories = new List<Category>
    {
        new(0, "Category A", new List<int> { 0 }),
        new(1, "Category B", new List<int> { 1 }),
        new(2, "Category C",  new List<int> { 2 })
    };

    private static readonly IList<Tenant> _tenants = new List<Tenant>
    {
        new(0, "State", new List<int> { 0, 1, 2}),
    };

    private static readonly List<Parameter> _queue = new();

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

    [HttpGet]
    [Route("subcategory")]
    public IActionResult GetSubcategory(int id)
    {
        return id < 0 || id >= _subcategories.Count ? BadRequest() : Ok(_subcategories[id]);
    }

    [HttpGet]
    [Route("category")]
    public IActionResult GetCategory(int id)
    {
        return id < 0 || id >= _categories.Count ? BadRequest() : Ok(_categories[id]);
    }

    [HttpGet]
    [Route("tenant")]
    public IActionResult GetTenant(int id)
    {
        return id < 0 || id >= _tenants.Count ? BadRequest() : Ok(_tenants[id]);
    }

    [HttpPost]
    [Route("queue")]
    public IActionResult PostQueue([FromBody] IList<Parameter> parameters)
    {
        _queue.AddRange(parameters);

        return Ok($"queued {parameters.Count} changes, now at {_queue.Count}");
    }

    /// <summary>
    /// Populate uses a url to get all the settings from a new world sight with a tenant and environment
    /// </summary>
    /// <param name="url"> The url from which we get our settings </param>
    /// <param name="tenantName"> The tenant to which the setting should be applied  </param>
    /// <param name="environmentName"> The environment to which the setting should be applied  </param>
    [HttpPost]
    [Route("populate")]
    public async Task<IActionResult> Populate([FromBody] string url, [FromQuery] string tenant, [FromQuery] string environment)
    {
        List<NewWorldSetting> settings;
        try
        {
            settings = await httpController.PopulateGetRequest(url);
        }
        except (ArgumentException)
        {
            return BadRequest($"Url {url} is invalid");
        }
        _dbController.PopulateHierarchy(settings, tenant, environment);
        return Ok($"Url {url} is valid");
    }
}
