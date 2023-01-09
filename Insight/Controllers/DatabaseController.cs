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

    public async Task<List<DatabaseSetting>> GetSettingsAsync()
    {
        return await _settingsService.GetAsync();
    }

    public async Task<DatabaseSetting?> GetSingleSettingAsync(string name)
    {
        return await _settingsService.GetByNameAsync(name);
    }

    public async Task<List<DatabaseSetting>> GetTenantSettingsAsync(string tenantName, string environment)
    {
        return await _settingsService.GetTenantsAsync(tenantName, environment);
    }

    public async Task DeleteAllAsync() =>
        await _settingsService.DeleteAllAsync();


    /// <summary>
    /// Populate Hierarchy takes a list of settings with a given tenant and environment and adds them to the database
    /// If a setting already exists, all relevant data is copied before updating
    /// </summary>
    /// <param name="settings"> All the settings to send to database </param>
    /// <param name="tenantName"> The tenant to which the setting should be applied  </param>
    /// <param name="environmentName"> The environment to which the setting should be applied  </param>
    // public async void PopulateHierarchy(List<NewWorldSetting> settings, string tenantName, string environmentName)
    // {
    //     //Iterate through all settings
    //     foreach (NewWorldSetting setting in settings)
    //     {    
    //         //Setup our new setting    
    //         DatabaseSetting dbSetting = new DatabaseSetting();
    //         dbSetting.Name = setting.Name;
    //         await _settingsService.CreateAsync(dbSetting);

    //         //Determine if setting exists
    //         var isSettingInDatabase = await _settingsService.GetByNameAsync(setting.Name);
           
    //         if (isSettingInDatabase != null)
    //         {
    //             //dbSetting.Parameters = setting.Parameters?.ToArray();
    //             //Get info from existing setting
    //             dbSetting.Id = isSettingInDatabase.Id;
    //             dbSetting.Category = isSettingInDatabase.Category;

    //             //Get info from existing setting while dealing with null pointers
    //             if (isSettingInDatabase.TenantNames != null)
    //             {
    //                 dbSetting.TenantNames = isSettingInDatabase.TenantNames;
    //                 dbSetting.TenantNames.Append(tenantName);
    //             }
    //             else
    //             {
    //                 dbSetting.TenantNames = new string[] { tenantName };
    //             }

    //             if (isSettingInDatabase.EnvironmentNames != null)
    //             {
    //                 dbSetting.EnvironmentNames = isSettingInDatabase.EnvironmentNames;
    //                 dbSetting.EnvironmentNames.Append(environmentName);
    //             }
    //             else
    //             {
    //                 dbSetting.EnvironmentNames = new string[] { environmentName };
    //             }
                
    //             //Update setting
    //             await _settingsService.UpdateByNameAsync(setting.Name, dbSetting);

    //         }
    //         else
    //         {
    //             //Create new setting
    //             dbSetting.TenantNames.SetValue(tenantName, 0);
    //             dbSetting.EnvironmentNames.SetValue(environmentName, 0);
    //             await _settingsService.CreateAsync(dbSetting);
    //         }
    //     }
    // }
    public async void PopulateHierarchy(List<NewWorldSetting> settings, string tenantName, string environmentName)
    {
        //Iterate through all settings
        foreach (NewWorldSetting setting in settings)
        {    
            //Setup our new setting    
            DatabaseSetting dbSetting = new DatabaseSetting();
            DatabaseTenant dbTenant = new DatabaseTenant();
            Console.WriteLine(setting.Tenant);

            dbSetting.Name = setting.Name;
            await _settingsService.CreateAsync(dbSetting);
            
            if(setting.Tenant != null) {
                dbTenant.Name = setting.Tenant.Name;
                await _tenantService.CreateAsync(dbTenant);   
            }

            //Determine if setting exists
            var isSettingInDatabase = await _settingsService.GetByNameAsync(setting.Name);
           
            if (isSettingInDatabase != null)
            {
                //Get info from existing setting
                dbSetting.Id = isSettingInDatabase.Id;
                dbSetting.Category = isSettingInDatabase.Category;

                //Get info from existing setting while dealing with null pointers
                if (isSettingInDatabase.TenantNames != null)
                {
                    dbSetting.TenantNames = isSettingInDatabase.TenantNames;
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
                    dbSetting.Tenants = new DatabaseTenant[] { };
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
                dbSetting.TenantNames.SetValue(tenantName, 0);
                dbSetting.EnvironmentNames.SetValue(environmentName, 0);
                await _settingsService.CreateAsync(dbSetting);
            }

             //Do the same thing with the Tenant database
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
    }
    
}




