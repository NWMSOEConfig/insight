using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Insight.Services;
using Insight.Models;

namespace Insight.Controllers;

public class DataServer {
     private readonly DatabaseSettingsService _settingsService;
     private readonly DatabaseCommitService _commitService;
     private readonly DatabaseTenantService _tenantService;
     private readonly DatabaseQueuedChangeService _queuedChangeService;
     private readonly DatabaseUserService _userService;
     public DataServer(DatabaseSettingsService settingsService, DatabaseTenantService tenantService, DatabaseCommitService commitService,
        DatabaseQueuedChangeService databaseQueuedChangeService, DatabaseUserService userService) {
        _settingsService = settingsService;
        _commitService = commitService;
        _tenantService = tenantService;
        _userService = userService;
        _queuedChangeService = databaseQueuedChangeService;
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
        var lastPulled = tenant?.EnvironmentLastPulled?[environmentName];
        // if (lastPulled is not null && lastPulled.Value.AddSeconds(300) > DateTime.UtcNow) {
        //     return lastPulled.Value;
        // }

        //Iterate through all settings
        foreach (NewWorldSetting setting in settings)
        {    
            //Setup our new setting    
            DatabaseSetting dbSetting = new DatabaseSetting();
            DatabaseTenant dbTenant = new DatabaseTenant();
            dbSetting.Name = setting.Name;
            await _settingsService.CreateAsync(dbSetting);
            

            //Determine if setting exists
            var isSettingInDatabase = await _settingsService.GetByNameAsync(setting.Name);
           
            if (isSettingInDatabase != null)
            {
                // dbSetting.Parameters = setting.Parameters?.ToArray();
                //Get info from existing setting
                dbSetting.Id = isSettingInDatabase.Id;
                dbSetting.Category = isSettingInDatabase.Category;

                //Get info from existing setting while dealing with null pointers
                if (isSettingInDatabase.TenantNames != null)
                {
                    dbSetting.TenantNames = isSettingInDatabase.TenantNames;
                    dbSetting.TenantNames.Append(tenantName);
                }
                else
                {
                    dbSetting.TenantNames = new string[] { tenantName };
                }

                if(isSettingInDatabase.Tenants != null)
                {
                    dbSetting.Tenants = isSettingInDatabase.Tenants;
                }
                else
                {
                    dbTenant = new DatabaseTenant();
                    dbTenant.Environment = new string[] { environmentName };
                    dbTenant.Name = tenantName;
                    dbSetting.Tenants = new DatabaseTenant[] { dbTenant };
                }

                if (isSettingInDatabase.EnvironmentNames != null)
                {
                    dbSetting.EnvironmentNames = isSettingInDatabase.EnvironmentNames;
                    dbSetting.EnvironmentNames.Append(environmentName);
                }
                else
                {
                    dbSetting.EnvironmentNames = new string[] { environmentName };
                }
                
                //Update setting
                await _settingsService.UpdateByNameAsync(setting.Name, dbSetting);

            }
            else
            {
                //Create new setting
                if (dbSetting.TenantNames is null)
                {
                    dbSetting.TenantNames = new string[] { string.Empty };
                }
                dbSetting.TenantNames.SetValue(tenantName, 0);
                if (dbSetting.EnvironmentNames is null)
                {
                    dbSetting.EnvironmentNames = new string[] { string.Empty };
                }
                dbSetting.EnvironmentNames.SetValue(environmentName, 0);
                await _settingsService.CreateAsync(dbSetting);
            }

             var isTenantInDatabase = await _tenantService.GetByNameAsync(tenantName);

            if(isTenantInDatabase != null)
            {
                if(isTenantInDatabase.Environment != null)
                {
                    dbTenant.Environment = isTenantInDatabase.Environment;
                }
                else
                {
                    dbTenant.Environment = new string[] { environmentName };
                }

                await _tenantService.UpdateByNameAsync(tenantName, dbTenant);
            }
        }

        

        lastPulled = DateTime.UtcNow;

        // if (tenant is null)
        // {
        //     tenant = new DatabaseTenant
        //     {
        //         Name = tenantName,
        //     };
        //     await _tenantService.CreateAsync(tenant);
        // }

        // if (tenant.EnvironmentLastPulled is null)
        // {
        //     tenant.EnvironmentLastPulled = new();
        // }

        // tenant.EnvironmentLastPulled[environmentName] = lastPulled.Value;
        // await _tenantService.UpdateAsync(tenant.Id, tenant);

        return lastPulled.Value;
    }
}
