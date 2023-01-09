using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Insight.Models;

namespace Insight.Controllers;

[ApiController]
[Route("api/database")]
public class UserController : ControllerBase {
    private DataServer _dbController;

    public UserController(DataServer databaseController) =>
        _dbController = databaseController;

    [HttpGet("environments/{environment}")]
    public async Task<List<DatabaseSetting>> environmentContext(string environment) 
    {
        List<DatabaseSetting> vals = new List<DatabaseSetting>();

        vals = await _dbController.GetEnvironmentSettingsAsync(environment);

        return vals;
    }

    [HttpGet("tenants/{tenant}")]
    public async Task<List<DatabaseSetting>> tenantContext(string environment)
    {
        return await _dbController.GetTenantSettingsAsync(environment);
    }

    [HttpGet]
    public async Task<List<DatabaseSetting>> getDatabaseSettings()
    {
        List<DatabaseSetting> DatabaseSettings = new List<DatabaseSetting>();

        DatabaseSettings = await _dbController.GetSettingsAsync();

        return DatabaseSettings;
    }

    [HttpGet("{name}")]
    public async Task<DatabaseSetting?> getSingleDatabaseSetting(string name)
    {
        return await _dbController.GetSingleSettingAsync(name);
    }
        
}

[Route("api/[controller]")]
public class DataController : ControllerBase
{
    private HttpController httpController = new HttpController();
    private DataServer _dbController;

    public DataController(DataServer databaseController) =>
        _dbController = databaseController;

    private static readonly IList<NewWorldSetting> _settings = new List<NewWorldSetting>
    {
        new("Foo")
        {
            Parameters = new List<Parameter>
            {
                new("Enabled", true, true),
                new("Foo", 123, true),
            },
        },
        new("Bar")
        {
            Parameters = new List<Parameter>
            {
                new("Bar", "Text", true),
            },
        },
        new("Baz")
        {
            Parameters = new List<Parameter>
            {
                new("Baz", "a@b.com", true),
            },
        },
    };

    private static readonly IList<Subcategory> _subcategories = new List<Subcategory>
    {
        new(0, "Subcategory 1", new List<string> { "Foo" }),
        new(1, "Subcategory 2", new List<string> { "Bar", "ActionItemCaseAssignmentEnabled" }),
        new(2, "Subcategory 3", new List<string> { "Baz" })
    };

    private static readonly IList<Category> _categories = new List<Category>
    {
        new(0, "Category A", new List<int> { 0 }),
        new(1, "Category B", new List<int> { 1 }),
        new(2, "Category C", new List<int> { 2 })
    };

    private static readonly IList<Tenant> _tenants = new List<Tenant>
    {
        new(0, "State", new List<int> { 0, 1, 2 }),
    };

    private static readonly List<QueueEntry> _queue = new();

    
    /// <summary>
    /// This method will eventually get a setting from the saved queue. It is presently being mocked.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public NewWorldSetting GetQueuedSetting(string name)
    {
        var setting = _settings.FirstOrDefault(s => s.Name == name);

        return setting;
    }
    /// <summary>
    /// This method retrieves setting information to display to the editor.
    /// If the setting has been modified it gets queued information instead.
    /// #TODO: Add tenant and environment filters, and get URL from environment
    /// </summary>
    /// <param name="name"></param> The setting name to search for.
    /// <returns></returns>
    [HttpGet]
    [Route("livesetting")]
    public async Task<IActionResult> GetSettingAsync(string name)
    {
        string url="https://pauat.newworldnow.com/v7/api/applicationsettings/";
        List<NewWorldSetting> settings;
        var setting = GetQueuedSetting(name);
        if(setting == null){
            try
            {
                settings = await httpController.PopulateGetRequest(url);
            }catch (ArgumentException)
            {
                return BadRequest($"Url {url} is invalid");
            }
            setting = settings.FirstOrDefault(s => s.Name == name);
        }

        return setting is null ? BadRequest() : Ok(setting);
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

    /// <summary>
    /// Add a modified setting to the batch of queued setting changes.
    /// </summary>
    /// <param name="setting">the setting to add to the batch</param>
    /// <returns>400 Bad Request if passed setting is invalid, else 200 OK</returns>
    [HttpPost]
    [Route("queue")]
    public IActionResult PostQueue([FromBody] NewWorldSetting setting)
    {
        var originalSetting = _settings.FirstOrDefault(s => s.Name == setting.Name);

        if (originalSetting is null)
        {
            return BadRequest("must have a valid setting name");
        }

        if (originalSetting.Parameters is null || originalSetting.Parameters.Count == 0 || setting.Parameters is null || setting.Parameters.Count == 0)
        {
            return BadRequest("setting definition must have parameters");
        }

        if (setting.Parameters.Any(parameter => !originalSetting.Parameters.Any(p => p.Name == parameter.Name)))
        {
            return BadRequest("all parameters must be part of the setting");
        }

        if (setting.Parameters.GroupBy(parameter => parameter.Name).Any(group => group.Count() > 1))
        {
            return BadRequest("cannot have duplicate parameters");
        }

        var queuer = Request.HttpContext.Connection.RemoteIpAddress.ToString();
        var entry = new QueueEntry(setting.Name, originalSetting.Parameters, setting.Parameters, queuer);
        _queue.Add(entry);

        return Ok($"queued {setting.Parameters.Count} parameter(s) for this change, now at {_queue.Count} setting(s) queued");
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
        catch (ArgumentException)
        {
            return BadRequest($"Url {url} is invalid");
        }

        var lastPulled = await _dbController.PopulateHierarchy(settings, tenant, environment);

        return Ok(lastPulled);
    }
}





