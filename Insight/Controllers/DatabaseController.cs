using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Insight.Services;
using Insight.Models;

namespace Insight.Controllers;

public class DataServer {
    private readonly DatabaseSettingsService _settingsService;
    private readonly DatabaseCommitService _commitService;
    private readonly DatabaseTenantService _tenantService;
    public DatabaseQueuedChangeService QueuedChangeService { get; private init; }
    private readonly DatabaseUserService _userService;
    public DataServer(DatabaseSettingsService settingsService, DatabaseTenantService tenantService, DatabaseCommitService commitService,
        DatabaseQueuedChangeService databaseQueuedChangeService, DatabaseUserService userService) {
        _settingsService = settingsService;
        _commitService = commitService;
        _tenantService = tenantService;
        _userService = userService;
        QueuedChangeService = databaseQueuedChangeService;
    }

    public async Task<List<DatabaseSetting>> GetEnvironmentSettingsAsync(string tenantName)
    {
        
        return await _settingsService.GetEnvironmentAsync(tenantName);
    }

    public async Task<List<DatabaseSetting>> GetTenantSettingsAsync(string tenantName, string environmentName)
    {
        return await _settingsService.GetTenantsAsync(tenantName, environmentName);
    }

    public async Task<List<DatabaseSetting>> GetSettingsAsync()
    {
        return await _settingsService.GetAsync();
    }

    public async Task<DatabaseSetting?> GetSingleSettingAsync(string name)
    {
        return await _settingsService.GetByNameAsync(name);
    }

    public async Task DeleteAllAsync() =>
        await _settingsService.DeleteAllAsync();

    /// <summary>
    /// Populate Hierarchy takes a list of settings with a given tenant and environment and adds them to the database.
    /// If a setting already exists, all relevant data is copied before updating.
    /// The EnvironmentLastPulled for the tenant's environment is then set to the current time.
    /// </summary>
    /// <param name="settings"> All the settings to send to database </param>
    /// <param name="tenantName"> The tenant to which the setting should be applied  </param>
    /// <param name="environmentName"> The environment to which the setting should be applied  </param>
    /// <returns>The new EnvironmentLastPulled time</returns>
    public async Task<DateTime> PopulateHierarchy(List<NewWorldSetting> settings, string tenantName, string environmentName)
    {
        // If we just did this already, don't update yet.
        var tenant = await _tenantService.GetCategoryAsync(tenantName);
        DateTime? lastPulled = tenant?.EnvironmentLastPulled?.ContainsKey(environmentName) ?? false
            ? tenant.EnvironmentLastPulled[environmentName] : null;
        if (lastPulled is not null && lastPulled.Value.AddSeconds(300) > DateTime.UtcNow) {
            return lastPulled.Value;
        }

        var dbSettings = await _settingsService.GetAsync();
        var newSettings = new List<DatabaseSetting>();

        // Iterate through all settings
        settings.ForEach(setting =>
        {
            // Determine if setting exists
            var dbSetting = dbSettings.FirstOrDefault(s => s.Name == setting.Name);

            if (dbSetting is not null)
            {
                // Update setting
                dbSetting.Parameters = setting.Parameters?.ToArray();

                if (dbSetting.TenantNames is null)
                    dbSetting.TenantNames = new string[] { tenantName };
                else if (!dbSetting.TenantNames.Contains(tenantName))
                    dbSetting.TenantNames.Append(tenantName);

                if (dbSetting.Tenants is null)
                {
                    dbSetting.Tenants = new DatabaseTenant[]
                    {
                        new DatabaseTenant
                        {
                            Environment = new string[] { environmentName },
                            Name = tenantName,
                        },
                    };
                }
                else if (!dbSetting.Tenants.Any(tenant => tenant.Name == tenantName))
                {
                    var list = dbSetting.Tenants.ToList();
                    list.Add(new DatabaseTenant
                    {
                        Environment = new string[] { environmentName },
                        Name = tenantName,
                    });
                    dbSetting.Tenants = list.ToArray();
                }

                if (dbSetting.EnvironmentNames is null)
                    dbSetting.EnvironmentNames = new string[] { environmentName };
                else if (!dbSetting.EnvironmentNames.Contains(environmentName))
                    dbSetting.EnvironmentNames.Append(environmentName);
            }
            else
            {
                // Setup our new setting    
                var newSetting = new DatabaseSetting
                {
                    Name = setting.Name,
                    Parameters = setting.Parameters?.ToArray(),
                    TenantNames = new string[] { tenantName },
                    EnvironmentNames = new string[] { environmentName },
                    Tenants = new DatabaseTenant[]
                    {
                        new DatabaseTenant
                        {
                            Environment = new string[] { environmentName },
                            Name = tenantName,
                        },
                    },
            };

                newSettings.Add(newSetting);
            }
        });

        if (dbSettings.Count > 0)
            await _settingsService.UpdateManyAsync(dbSettings);
        if (newSettings.Count > 0)
            await _settingsService.CreateManyAsync(newSettings);

        lastPulled = DateTime.UtcNow;

        if (tenant is null)
        {
            tenant = new DatabaseTenant
            {
                Name = tenantName,
                Environment = new string[] { environmentName },
            };
            await _tenantService.CreateAsync(tenant);
        }

        if (tenant.EnvironmentLastPulled is null)
        {
            tenant.EnvironmentLastPulled = new();
        }

        if (tenant.Environment is null)
        {
            tenant.Environment = new string[] { environmentName };
        }
        else if (!tenant.Environment.Contains(environmentName))
        {
            var list = tenant.Environment.ToList();
            list.Add(environmentName);
            tenant.Environment = list.ToArray();
        }

        tenant.EnvironmentLastPulled[environmentName] = lastPulled.Value;
        await _tenantService.UpdateAsync(tenant.Id, tenant);

        return lastPulled.Value;
    }

    
    public async Task<Commit?> CreateCommitFromQueue(string user, string tenantName, string environmentName ){
        Commit myCommit = new Commit();
        QueuedChange? queuedChange = await QueuedChangeService.GetAsync(user, tenantName, environmentName);
        if(queuedChange!=null){
            myCommit.QueueChange = queuedChange;
            myCommit.Time = DateTime.UtcNow;
            await _commitService.CreateAsync(myCommit);
            return myCommit;
        }

        return null;
    }    
}
