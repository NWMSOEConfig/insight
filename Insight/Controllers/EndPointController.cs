using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Insight.Models;
using System.Text.Json;

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

    private static readonly IList<Subcategory> _subcategories = new List<Subcategory> { };

    private static readonly IList<Category> _categories = new List<Category> { };

    private static readonly IList<Tenant> _tenants = new List<Tenant> { };

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
        var queue = await _dbController.GetQueue(userName, tenantName, environmentName);

        if (queue is null)
            return null;

        var dbSetting = queue.Settings.FirstOrDefault(s => s.newSetting.Name == settingName);

        if (dbSetting?.newSetting is null)
            return null;

        var setting = new NewWorldSetting(dbSetting.newSetting.Name)
        {
            Parameters = dbSetting.newSetting.Parameters?.ToList(),
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
        var userName = Request?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        string? url = await _dbController.GetUrlFromTenant(tenantName, environmentName);
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

    [HttpPost]
    [Route("publish")]
    public async Task<Commit?> PublishSettingsAsync([FromQuery] string user, [FromQuery] string tenant, [FromQuery] string environment)
    {
        string url = "https://pauat.newworldnow.com/v7/api/updatesetting/";
        Commit? commit;
        QueuedChange? change = await _dbController.GetQueue(user, tenant, environment);
        if (change != null)
        {
            httpController.MakePostRequest(change, url);
        }
        else
        {
            Console.WriteLine("Queue not found");
        }
        commit = await _dbController.CreateCommitFromQueue(user, tenant, environment);
        return commit;

    }

    [HttpGet]
    [Route("commits")]
    public async Task<IActionResult> GetCommitsAsync(string tenantName, string environmentName)
    {

        return Ok(JsonSerializer.Serialize((await _dbController.GetCommitsAsync(tenantName, environmentName))));
    }

    [HttpGet]
    [Route("dbsettings")]
    public async Task<IActionResult> GetAllSettingsAsync(string tenantName, string environmentName)
    {
        return Ok(JsonSerializer.Serialize((await _dbController.GetTenantSettingsAsync(tenantName, environmentName))));
    }

    [HttpGet]
    [Route("dbtenants")]
    public async Task<IActionResult> GetAllTenantsAync(string tenantName, string environmentName)
    {
        return Ok(JsonSerializer.Serialize((await _dbController.GetAllTenantsAsync())));
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

    [HttpGet]
    [Route("commit")]
    public async Task<IActionResult> GetCommitAsync([FromQuery] string tenantName, [FromQuery] string environmentName, [FromQuery] int id)
    {
        return Ok(JsonSerializer.Serialize((await _dbController.GetCommit(tenantName, environmentName, id))));
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
        var dbQueue = await _dbController.GetQueue(userName, tenantName, environmentName);

        if (dbQueue is null)
        {
            return new NewWorldSetting[] { };
        }
        else
        {
            List<NewWorldSetting> updatedSettings = new List<NewWorldSetting>();
            foreach(var setting in dbQueue.Settings) {
                updatedSettings.Add(new NewWorldSetting(setting.newSetting.Name) {
                    Parameters = setting.newSetting.Parameters?.ToList(),
                });
            }
            return updatedSettings;
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
        var dbQueue = await _dbController.GetQueue(userName, tenantName, environmentName);

        if (dbQueue is null)
            return NotFound();

        bool success;

        var settings = dbQueue.Settings;
        success = settings.RemoveAll(setting => setting.newSetting.Name == settingName) > 0;
        for(int i = 0; i < settings.Count; i++) {
            if(settings[i].newSetting.Name == settingName) {
                settings[i] = new ChangedSetting { };
                success = true;
            }
        }

        dbQueue.Settings = settings;

        await _dbController.CreateOrUpdateQueue(dbQueue);

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
        var userName = Request.HttpContext.Connection.RemoteIpAddress.ToString();

        try
        {
            var queue = await _dbController.EnqueueSetting(setting, tenantName, environmentName, userName);
            return Ok($"queued {setting.Parameters?.Count} parameter(s) for this change, now at {queue.Settings.Count()} setting(s) queued");
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }

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

    [HttpDelete]
    [Route("DeleteAllQueuedChanges")]
    public Task DeleteQueuedChanges() =>
        _dbController.DeleteAllQueuedChangesAsync();
}






