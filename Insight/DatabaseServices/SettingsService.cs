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
        await collection.Find(x => x.Id == id).FirstOrDefaultAsync();

     public async Task<DatabaseSetting?> GetByNameAsync(string name) =>
        await collection.Find(x => x.Name == name).FirstOrDefaultAsync();
    
    public async Task<List<DatabaseSetting>> GetEnvironmentAsync(string environmentName)
    {
        List<DatabaseSetting> settings = new List<DatabaseSetting>();
        List<DatabaseSetting> matched_settings = new List<DatabaseSetting>();
        List<DatabaseEnvironment> environments = new List<DatabaseEnvironment>();

        settings = await GetAsync();
        
        settings.ForEach(x => {
            if(x.Environments != null)
                environments = x.Environments.ToList();
            if(environments.Any(environment => environment.Name == environmentName))
                matched_settings.Add(x);
        });
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

        settings.ForEach(x => {
            if(x.Tenants != null)
                tenants = x.Tenants.ToList();

            tenants.ForEach(y => {
                if(y.Environments != null)
                    environments = y.Environments.ToList();
                environments.ForEach(z => {
                    if(z.Name == environmentName && y.Name == tenantName)
                    {
                        matched_settings.Add(x);
                        return;
                    }
                });
            });
        });

        return matched_settings;
    }

    public async Task UpdateAsync(string id, DatabaseSetting updatedSetting) =>
        await collection.ReplaceOneAsync(x => x.Id == id, updatedSetting);

    public async Task UpdateByNameAsync(string name, DatabaseSetting updatedSetting) =>
        await collection.ReplaceOneAsync(x => x.Name == name, updatedSetting);

    public async Task UpdateManyAsync(IEnumerable<DatabaseSetting> settings)
    {
        var updates = settings.Select(setting =>
        {
            var filter = Builders<DatabaseSetting>.Filter.Eq(x => x.Id, setting.Id);
            return new ReplaceOneModel<DatabaseSetting>(filter, setting);
        });
        await collection.BulkWriteAsync(updates);
    }

    public async Task RemoveAsync(string id) =>
        await collection.DeleteOneAsync(x => x.Id == id);
    
     public async Task DeleteAllAsync() =>
        await collection.DeleteManyAsync(_ => true);
        
}