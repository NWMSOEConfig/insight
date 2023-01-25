using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Insight.Models;

namespace Insight.Controllers;

[ApiController]
[Route("api/database")]
public class UserController : ControllerBase
{
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

    [HttpGet("tenants/{tenant}/{environment}")]
    public async Task<List<DatabaseSetting>> tenantContext(string tenant, string environment)
    {
        return await _dbController.GetTenantSettingsAsync(tenant, environment);
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

    private static readonly IList<Subcategory> _subcategories = new List<Subcategory>
    {
        new(0, "Subcategory 1", new List<string> { "ActionItemCaseAssignmentEnabled", "ActionItemInvestmentMaterialsDescriptionMaxCharacterLength"}),
        new(1, "Subcategory 2", new List<string> { "ActionItemListDaysSinceCompletion", "ActivateStudyPlanNoteActivityID" }),
        new(2, "Subcategory 3", new List<string> { "AllowCaseEditOnExistingActionItems" })
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

    /// <summary>
    /// Attempt to fetch a setting from the saved queue for this tenant/environment/user combination.
    /// </summary>
    /// <param name="settingName">name of the setting</param>
    /// <param name="userName">name of the user</param>
    /// <param name="tenantName">name of the tenant</param>
    /// <param name="environmentName">name of the environment</param>
    /// <returns>the setting, or null if not queued for this tenant/environment/user</returns>
    public async Task<NewWorldSetting?> GetQueuedSetting(string settingName, string userName, string tenantName, string environmentName)
    {
        var queue = await _dbController.QueuedChangeService.GetAsync(userName, tenantName, environmentName);

        if (queue is null)
            return null;

        var dbSetting = queue.Settings.FirstOrDefault(s => s.Name == settingName);

        if (dbSetting is null)
            return null;

        var setting = new NewWorldSetting(dbSetting.Name)
        {
            Parameters = dbSetting.Parameters?.ToList(),
            Category = null,
            Tenant = null, // TODO
        };

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
    public async Task<IActionResult> GetSettingAsync(string settingName, string tenantName, string environmentName)
    {
        var userName = Request.HttpContext.Connection.RemoteIpAddress.ToString();
        string url = "https://pauat.newworldnow.com/v7/api/applicationsettings/";
        List<NewWorldSetting> settings;
        var setting = await GetQueuedSetting(settingName, userName, tenantName, environmentName);
        if (setting == null)
        {
            try
            {
                settings = await httpController.PopulateGetRequest(url);
            }
            catch (ArgumentException)
            {
                return BadRequest($"Url {url} is invalid");
            }
            setting = settings.FirstOrDefault(s => s.Name == settingName);
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
    /// Get the user's current setting queue.
    /// </summary>
    /// <param name="tenantName">the tenant the queue is for</param>
    /// <param name="environmentName">the environment the queue is for</param>
    /// <returns>the current setting queue</returns>
    [HttpGet]
    [Route("queue")]
    public async Task<IEnumerable<NewWorldSetting>> GetQueue([FromQuery] string tenantName, [FromQuery] string environmentName)
    {
        var userName = Request.HttpContext.Connection.RemoteIpAddress.ToString();
        var dbQueue = await _dbController.QueuedChangeService.GetAsync(userName, tenantName, environmentName);

        if (dbQueue is null)
        {
            return new NewWorldSetting[] {};
        }
        else
        {
            return dbQueue.Settings.Select(setting => new NewWorldSetting(setting.Name)
            {
                Parameters = setting.Parameters?.ToList(),
            });
        }
    }

    /// <summary>
    /// Delete a setting from a user's queue.
    /// </summary>
    /// <param name="tenantName">the tenant the queue is for</param>
    /// <param name="environmentName">the environment the queue is for</param>
    /// <param name="settingName">the setting to remove from the queue</param>
    /// <returns>204 No Content if removed, else 404 Not Found if setting not in queue</returns>
    [HttpDelete]
    [Route("queue")]
    public async Task<IActionResult> DeleteQueuedSetting([FromQuery] string tenantName, [FromQuery] string environmentName, [FromQuery] string settingName)
    {
        var userName = Request.HttpContext.Connection.RemoteIpAddress.ToString();
        var dbQueue = await _dbController.QueuedChangeService.GetAsync(userName, tenantName, environmentName);

        if (dbQueue is null)
            return NotFound();

        bool success;

        var settings = dbQueue.Settings.ToList();
        success = settings.RemoveAll(setting => setting.Name == settingName) > 0;
        dbQueue.Settings = settings.ToArray();

        var oldSettings = dbQueue.OriginalSettings.ToList();
        success &= oldSettings.RemoveAll(setting => setting.Name == settingName) > 0;
        dbQueue.OriginalSettings = oldSettings.ToArray();

        await _dbController.QueuedChangeService.CreateOrUpdateAsync(dbQueue);

        return success ? NoContent() : NotFound();
    }

    /// <summary>
    /// Add a modified setting to the batch of queued setting changes.
    /// </summary>
    /// <param name="setting">the setting to add to the batch</param>
    /// <param name="tenantName">the tenant to queue for</param>
    /// <param name="environmentName">the environment to queue for</param>
    /// <returns>400 Bad Request if passed setting is invalid, else 200 OK</returns>
    [HttpPost]
    [Route("queue")]
    public async Task<IActionResult> PostQueue([FromBody] NewWorldSetting setting, [FromQuery] string tenantName, [FromQuery] string environmentName)
    {
        var originalSetting = await _dbController.GetSingleSettingAsync(setting.Name);

        if (originalSetting is null)
        {
            return BadRequest("must have a valid setting name");
        }

        if (originalSetting.Parameters is null || originalSetting.Parameters.Length == 0 || setting.Parameters is null || setting.Parameters.Count == 0)
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

        var entry = await _dbController.QueuedChangeService.GetAsync(queuer, tenantName, environmentName);

        if (entry is null)
        {
            entry = new QueuedChange
            {
                Settings = new DatabaseSetting[]
                {
                    new DatabaseSetting
                    {
                        Name = setting.Name,
                        Parameters = setting.Parameters.ToArray(),
                    }
                },
                OriginalSettings = new DatabaseSetting[] { originalSetting },
                User = new User
                {
                    Name = queuer,
                },
                Tenant = new DatabaseTenant
                {
                    Name = tenantName,
                },
                Environment = environmentName,
            };
        }
        else
        {
            var settings = entry.Settings.ToList();
            var oldSettings = entry.OriginalSettings.ToList();
            var newSetting = new DatabaseSetting
            {
                Name = setting.Name,
                Parameters = setting.Parameters.ToArray(),
            };

            // If this setting was added to this batch sometime earlier,
            // don't make a duplicate queue entry.
            foreach (var s in settings)
            {
                if (s.Name == setting.Name)
                {
                    settings.Remove(s);
                    break;
                }
            }
            settings.Add(newSetting);
            entry.Settings = settings.ToArray();

            foreach (var s in entry.OriginalSettings)
            {
                if (s.Name == setting.Name)
                {
                    settings.Remove(s);
                    break;
                }
            }
            oldSettings.Add(originalSetting);
            entry.OriginalSettings = oldSettings.ToArray();
        }

        await _dbController.QueuedChangeService.CreateOrUpdateAsync(entry);

        return Ok($"queued {setting.Parameters.Count} parameter(s) for this change, now at {entry.Settings.Count()} setting(s) queued");
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

        var lastPulled = await _dbController.PopulateHierarchy(settings, tenant, environment, url);

        return Ok(lastPulled);
    }

    [HttpDelete]
    [Route("DeleteAllSettings")]
    public Task DeleteSettings() =>
        _dbController.DeleteAllAsync();

    [HttpDelete]
    [Route("DeleteAllTenants")]
    public Task DeleteTenants() =>
        _dbController.DeleteAllTenantsAsync();
}






