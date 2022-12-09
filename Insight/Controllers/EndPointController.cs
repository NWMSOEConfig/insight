using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Insight.Services;
using Insight.Models;

namespace Insight.Controllers;

[ApiController]
[Route("api/database")]
public class UserController : ControllerBase {
    private DataServer _dbController;
     public UserController(DataServer databaseController) =>
        _dbController = databaseController;


    [HttpGet("{environment}")]
    public async Task<List<DatabaseSetting>> environmentContext(string environment) 
    {
        List<DatabaseSetting> vals = new List<DatabaseSetting>();

        vals = await _dbController.GetEnvironmentSettingsAsync(environment);

        return vals;
    }

    [HttpGet]
    public async Task<List<DatabaseSetting>> getDatabaseSettings()
    {
        List<DatabaseSetting> DatabaseSettings = new List<DatabaseSetting>();

        DatabaseSettings = await _dbController.GetSettingsAsync();

        return DatabaseSettings;
    }
        
}

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

    private static readonly List<QueueEntry> _queue = new();

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
    public IActionResult PostQueue(int settingId, [FromBody] IList<Parameter> parameters)
    {
        if (settingId < 0 || settingId >= _settings.Count)
        {
            return BadRequest("must have a valid setting ID");
        }

        if (parameters.Any(parameter => !_settings[settingId].ParameterIds.Contains(parameter.Id)))
        {
            return BadRequest("all parameters must be part of the setting");
        }

        if (parameters.GroupBy(parameter => parameter.Id).Any(group => group.Count() > 1))
        {
            return BadRequest("cannot have duplicate parameters");
        }

        var originals = parameters.Select(parameter => _parameters[parameter.Id]).ToList();
        var queuer = Request.HttpContext.Connection.RemoteIpAddress.ToString();
        var entry = new QueueEntry(settingId, originals, parameters, queuer);
        _queue.Add(entry);

        return Ok($"queued {parameters.Count} parameter(s) for this change, now at {_queue.Count} setting(s) queued");
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
        }catch (ArgumentException)
        {
            return BadRequest($"Url {url} is invalid");
        }
        _dbController.PopulateHierarchy(settings, tenant, environment);
        return Ok($"Url {url} is valid");
    }
}





