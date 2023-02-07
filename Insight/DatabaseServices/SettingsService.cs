using Insight.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Insight.Services;

public class DatabaseSettingsService : ServiceParent<DatabaseSetting>
{
    public DatabaseSettingsService()
    {
        var mongoClient = new MongoClient("mongodb+srv://dbTestUser:friedegg@new-world.tmynaas.mongodb.net/?retryWrites=true&w=majority");

        var mongoDatabase = mongoClient.GetDatabase("Configurations");

        collection = mongoDatabase.GetCollection<DatabaseSetting>("Settings");
    }

    public async Task<DatabaseSetting?> GetAsync(string id) =>
        await collection.Find(setting => setting.Id == id).FirstOrDefaultAsync();

     public async Task<DatabaseSetting?> GetByNameAsync(string name) =>
        await collection.Find(setting => setting.Name == name).FirstOrDefaultAsync();
    
    public async Task<List<DatabaseSetting>> GetEnvironmentAsync(string environmentName)
    {
        List<DatabaseSetting> settings = new List<DatabaseSetting>();
        List<DatabaseSetting> matched_settings = new List<DatabaseSetting>();
        List<DatabaseEnvironment> environments = new List<DatabaseEnvironment>();

        settings = await GetAsync();
        
        settings.ForEach(setting => {
            if(setting.Environments != null)
                environments = setting.Environments.ToList();
            if(environments.Any(environment => environment.Name == environmentName))
                matched_settings.Add(setting);
        });
        matched_settings.OrderBy(setting => setting.Category).ToList();
        return matched_settings;
    }

    public async Task<List<DatabaseSetting>> GetTenantsAsync(string tenantName, string environmentName)
    {
        // create placeholder lists to hold information and one list to return
        List<DatabaseSetting> settings = new List<DatabaseSetting>();
        List<DatabaseTenant> tenants = new List<DatabaseTenant>();
        List<DatabaseSetting> matched_settings = new List<DatabaseSetting>();
        List<DatabaseEnvironment> environments = new List<DatabaseEnvironment>();

        // get all settings
        settings = await GetAsync();

        settings.ForEach(setting => {
            if(setting.Tenants != null)
                tenants = setting.Tenants.ToList();

            tenants.ForEach(tenant => {
                if(tenant.Environments != null)
                    environments = tenant.Environments.ToList();
                environments.ForEach(environment => {
                    if(environment.Name == environmentName && tenant.Name == tenantName)
                    {
                        matched_settings.Add(setting);
                        return;
                    }
                });
            });
        });
        matched_settings.OrderBy(setting => setting.Category).ToList();
        return matched_settings;
    }

    public async Task UpdateAsync(string id, DatabaseSetting updatedSetting) =>
        await collection.ReplaceOneAsync(setting => setting.Id == id, updatedSetting);

    public async Task UpdateByNameAsync(string name, DatabaseSetting updatedSetting) =>
        await collection.ReplaceOneAsync(setting => setting.Name == name, updatedSetting);

    public async Task UpdateManyAsync(IEnumerable<DatabaseSetting> settings)
    {
        var updates = settings.Select(setting =>
        {
            var filter = Builders<DatabaseSetting>.Filter.Eq(setting => setting.Id, setting.Id);
            return new ReplaceOneModel<DatabaseSetting>(filter, setting);
        });
        await collection.BulkWriteAsync(updates);
    }

    public async Task RemoveAsync(string id) =>
        await collection.DeleteOneAsync(setting => setting.Id == id);
    
     public async Task DeleteAllAsync() =>
        await collection.DeleteManyAsync(_ => true);
        
}