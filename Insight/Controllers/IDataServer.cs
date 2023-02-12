using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Insight.Services;
using Insight.Models;

namespace Insight.Controllers;

public interface IDataServer
{

    Task<List<DatabaseSetting>> GetEnvironmentSettingsAsync(string tenantName);

    Task<List<DatabaseSetting>> GetTenantSettingsAsync(string tenantName, string environmentName);

    Task<List<DatabaseSetting>> GetSettingsAsync();

    Task<DatabaseSetting?> GetSingleSettingAsync(string name);

    Task DeleteAllAsync();

    Task DeleteAllTenantsAsync();

    Task<DateTime> PopulateHierarchy(List<NewWorldSetting> settings, string tenantName, string environmentName, string url);

    Task<QueuedChange?> GetQueue(string userName, string tenantName, string environmentName);

    Task CreateOrUpdateQueue(QueuedChange queue);
}
