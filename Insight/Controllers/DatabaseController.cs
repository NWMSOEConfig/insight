using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Insight.Services;
using Insight.Models;

namespace Insight.Controllers;

public class DataServer
{
    private readonly DatabaseSettingsService _settingsService;
    private readonly DatabaseCommitService _commitService;
    private readonly DatabaseTenantService _tenantService;
    private readonly DatabaseQueuedChangeService _queuedChangeService;
    private readonly DatabaseUserService _userService;
    private readonly DatabaseEnvironmentService _environmentService;
    public DataServer(DatabaseSettingsService settingsService, DatabaseTenantService tenantService, DatabaseCommitService commitService,
        DatabaseQueuedChangeService databaseQueuedChangeService, DatabaseUserService userService, DatabaseEnvironmentService environmentService)
    {
        _settingsService = settingsService;
        _commitService = commitService;
        _tenantService = tenantService;
        _userService = userService;
        _queuedChangeService = databaseQueuedChangeService;
        _environmentService = environmentService;
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

    public async Task<List<DatabaseTenant>> GetAllTenantsAsync()
    {
        return await _tenantService.GetAsync();
    }

    public async Task<DatabaseSetting?> GetSingleSettingAsync(string name)
    {
        return await _settingsService.GetByNameAsync(name);
    }

    public async Task DeleteAllAsync() =>
        await _settingsService.DeleteAllAsync();

    public async Task DeleteAllTenantsAsync() =>
        await _tenantService.DeleteAllAsync();

    public async Task DeleteAllQueuedChangesAsync() =>
        await _queuedChangeService.DeleteAllAsync();

    public async Task DeleteSettingFromQueue(string settingName) =>
        await _queuedChangeService.DeleteSingleSetting(settingName);


    /// <summary>
    /// Populate Hierarchy takes a list of settings with a given tenant and environment and adds them to the database.
    /// If a setting already exists, all relevant data is copied before updating.
    /// The EnvironmentLastPulled for the tenant's environment is then set to the current time.
    /// </summary>
    /// <param name="settings"> All the settings to send to database </param>
    /// <param name="tenantName"> The tenant to which the setting should be applied  </param>
    /// <param name="environmentName"> The environment to which the setting should be applied  </param>
    /// <returns>The new EnvironmentLastPulled time</returns>
    public async Task<DateTime> PopulateHierarchy(List<NewWorldSetting> settings, string tenantName, string environmentName, string url)
    {
        // If we just did this already, don't update yet.
        var environment = await _environmentService.GetNameAsync(environmentName);
        var tenant = await _tenantService.GetCategoryAsync(tenantName);
        DateTime? lastPulled = environment?.EnvironmentLastPulled?.ContainsKey(environmentName) ?? false
            ? environment.EnvironmentLastPulled[environmentName] : null;
        if (lastPulled is not null && lastPulled.Value.AddSeconds(300) > DateTime.UtcNow)
        {
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

                if (dbSetting.Environments is null)
                {
                    dbSetting.Environments = new DatabaseEnvironment[]
                    {
                        new DatabaseEnvironment
                        {
                            Name = environmentName,
                            Url = url
                        }
                    };
                }
                else if (!dbSetting.Environments.Any(environment => environment.Name == environmentName))
                {
                    var list = dbSetting.Environments.ToList();
                    list.Add(new DatabaseEnvironment
                    {
                        Name = environmentName,
                        Url = url
                    });
                    dbSetting.Environments = list.ToArray();
                }

                if (dbSetting.Tenants is null)
                {
                    dbSetting.Tenants = new DatabaseTenant[]
                    {
                        new DatabaseTenant
                        {
                            Name = tenantName,
                            Environments = new DatabaseEnvironment[] { new DatabaseEnvironment { Name = environmentName, Url = url }},
                        },
                    };
                }
                else if (!dbSetting.Tenants.Any(tenant => tenant.Name == tenantName))
                {
                    var list = dbSetting.Tenants.ToList();
                    list.Add(new DatabaseTenant
                    {
                        Name = tenantName,
                        Environments = new DatabaseEnvironment[] { new DatabaseEnvironment { Name = environmentName, Url = url } },
                    });
                    dbSetting.Tenants = list.ToArray();
                }
                dbSetting.Category = dbSetting.Name.Substring(0, 1);
            }
            else
            {
                // Setup our new setting    
                var newSetting = new DatabaseSetting
                {
                    Name = setting.Name,
                    Category = setting.Name.Substring(0, 1),
                    Parameters = setting.Parameters?.ToArray(),
                    Tenants = new DatabaseTenant[]
                    {
                        new DatabaseTenant
                        {
                            Name = tenantName,
                            Environments = new DatabaseEnvironment[] { new DatabaseEnvironment { Name = environmentName, Url = url }},
                        },
                    },
                    Environments = new DatabaseEnvironment[]
                    {
                        new DatabaseEnvironment
                        {
                            Name = environmentName,
                            Url = url,
                        }
                    }
                };

                newSettings.Add(newSetting);
            }
        });

        dbSettings.OrderBy(setting => setting.Category);
        newSettings.OrderBy(setting => setting.Category);

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
                Environments = new DatabaseEnvironment[] { new DatabaseEnvironment { Name = environmentName, Url = url } },
            };
            await _tenantService.CreateAsync(tenant);
        }

        if (environment is null)
        {
            environment = new DatabaseEnvironment
            {
                Name = environmentName,
                Url = url,
            };
            await _environmentService.CreateAsync(environment);
        }

        if (environment.EnvironmentLastPulled is null)
        {
            environment.EnvironmentLastPulled = new();
        }

        if (tenant.Environments is null)
        {
            tenant.Environments = new DatabaseEnvironment[] { new DatabaseEnvironment { Name = environmentName, Url = url } };
        }
        else if (!tenant.Environments.Any(environment => environment.Name == environmentName))
        {
            var list = tenant.Environments.ToList();
            list.Add(new DatabaseEnvironment { Name = environmentName, Url = url });
            tenant.Environments = list.ToArray();
        }

        if (lastPulled.Value != null)
        {
            environment.EnvironmentLastPulled[environmentName] = lastPulled.Value;
        }
        else
        {
            lastPulled = DateTime.Now;
            environment.EnvironmentLastPulled[environmentName] = lastPulled.Value;
        }
        await _tenantService.UpdateAsync(tenant.Id, tenant);

        return lastPulled.Value;
    }


    public async Task<Commit?> CreateCommitFromQueue(string user, string tenantName, string environmentName, string commitMessage, int ReferenceId)
    {
        Commit myCommit = new Commit();
        QueuedChange? queuedChange = await _queuedChangeService.GetAsync(user, tenantName, environmentName);
        if (queuedChange != null)
        {
            myCommit.QueueChange = queuedChange;
            myCommit.Time = DateTime.UtcNow;
            myCommit.ReferenceId = ReferenceId;
            myCommit.CommitMessage = commitMessage;
            await _commitService.CreateAsync(myCommit);
            return myCommit;
        }

        return null;
    }
    public async Task<List<Commit>> GetCommitsAsync(string tenantName, string environmentName)
        => await _commitService.GetCommitsAsync(tenantName, environmentName);

    /// <summary>
    /// Add a setting to a queue. If the queue for the specified combination
    /// of tenant/environment/user doesn't exist, create it.
    /// </summary>
    /// <param name="setting">setting to add</param>
    /// <param name="tenantName">tenant the queue is for</param>
    /// <param name="environmentName">environment the queue is for</param>
    /// <param name="userName">user the queue is for</param>
    /// <returns>the new state of the queue</returns>
    /// <exception cref="ArgumentException">the queue was not given a valid update</exception>
    public async Task<QueuedChange> EnqueueSetting(NewWorldSetting setting, string tenantName, string environmentName, string userName)
    {
        var originalSetting = await GetSingleSettingAsync(setting.Name);

        if (originalSetting is null)
            throw new ArgumentException("must have a valid setting name");

        if (originalSetting.Parameters is null || originalSetting.Parameters.Length == 0 || setting.Parameters is null || setting.Parameters.Count == 0)
            throw new ArgumentException("setting definition must have parameters");

        if (setting.Parameters.Any(parameter => !originalSetting.Parameters.Any(p => originalSetting.Name == setting.Name)))
            throw new ArgumentException("all parameters must be part of the setting");

        if (setting.Parameters.GroupBy(parameter => setting.Name).Any(group => group.Count() > 1))
            throw new ArgumentException("cannot have duplicate parameters");

        var entry = await _queuedChangeService.GetAsync(userName, tenantName, environmentName);

        if (entry is null)
        {
            entry = new QueuedChange
            {
                Settings = new List<ChangedSetting>
                {
                    new ChangedSetting {
                        oldSetting = new DatabaseSetting {
                            Name = originalSetting.Name,
                            Parameters = originalSetting.Parameters.ToArray(),
                            Tenants = originalSetting.Tenants,
                            Environments = originalSetting.Environments
                        },

                        newSetting = new DatabaseSetting {
                            Name = setting.Name,
                            Parameters = setting.Parameters.ToArray(),
                        }
                    }
                },
                // OriginalSettings = new DatabaseSetting[] { originalSetting },
                User = new User
                {
                    Name = userName,
                },
                Tenant = new DatabaseTenant
                {
                    Name = tenantName,
                    Environments = new DatabaseEnvironment[]
                    {
                        new DatabaseEnvironment
                        {
                            Name = environmentName,
                        },
                    },
                },
            };
        }
        else
        {
            var settings = entry.Settings.ToList();
            var newSetting = new DatabaseSetting
            {
                Name = setting.Name,
                Parameters = setting.Parameters.ToArray(),
            };

            // If this setting was added to this batch sometime earlier,
            // don't make a duplicate queue entry.
            foreach (var s in settings)
            {
                if (s.newSetting.Name == setting.Name)
                {
                    settings.Remove(s);
                    break;
                }
            }
            settings.Add(new ChangedSetting
            {
                oldSetting = originalSetting,
                newSetting = newSetting,
            });
            entry.Settings = settings;

            foreach (var s in settings)
            {
                if (s.oldSetting.Name == setting.Name)
                {
                    settings.Remove(s);
                    break;
                }
            }
            settings.Add(new ChangedSetting
            {
                oldSetting = originalSetting,
                newSetting = newSetting,
            });
        }

        await _queuedChangeService.CreateOrUpdateAsync(entry);

        return entry;
    }

    public Task<QueuedChange?> GetQueue(string userName, string tenantName, string environmentName)
        => _queuedChangeService.GetAsync(userName, tenantName, environmentName);


    public Task DeleteQueue(string queueId)
        => _queuedChangeService.RemoveAsync(queueId);

    public async Task<string?> GetUrlFromTenant(string tenantName, string environmentName)
    {
        DatabaseTenant? tenant = await _tenantService.GetByNameAsync(tenantName);

        if (tenant == null)
        {
            throw new ArgumentException("No such tenant exists");
        }

        foreach (DatabaseEnvironment e in tenant.Environments)
        {
            if (e.Name == environmentName)
            {
                return e.Url;
            }
        }

        throw new ArgumentException("No such environment exists");
    }
    public Task CreateOrUpdateQueue(QueuedChange queue)
        => _queuedChangeService.CreateOrUpdateAsync(queue);

    public async Task<Commit> GetCommit(string tenantName, string environmentName, int id)
        => await _commitService.GetCommitAsync(tenantName, environmentName, id);
}
