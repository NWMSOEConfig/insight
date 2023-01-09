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
    
    public async Task<List<DatabaseSetting>> GetEnvironmentAsync(string tenantId)
    {
        return await collection.Find(x => x.EnvironmentNames.Contains(tenantId)).ToListAsync();
    }

    public async Task DeleteAllAsync() =>
        await collection.DeleteManyAsync(_ => true);

    public async Task<List<DatabaseSetting>> GetTenantsAsync(string tenantName, string environment)
    {
        // create placeholder lists to hold information and one list to return
        List<DatabaseSetting> settings = new List<DatabaseSetting>();
        DatabaseTenant tenants = new DatabaseTenant();
        List<DatabaseSetting> matched_settings = new List<DatabaseSetting>();

        // get all settings
        settings = await GetAsync();

        settings.ForEach(x => {
            if(x.Tenants != null) {
                 tenants = x.Tenants;
                // search in each setting, for each tenant, to see of the environment name matches
                if(tenants.Environment != null && tenants.Name != null)
                    if(tenants.Environment.Contains(environment) && tenants.Name == tenantName)
                        matched_settings.Add(x);
            }
        });

        return matched_settings;
    }

    public async Task UpdateAsync(string id, DatabaseSetting updatedSetting) =>
        await collection.ReplaceOneAsync(x => x.Id == id, updatedSetting);

    public async Task UpdateByNameAsync(string name, DatabaseSetting updatedSetting) =>
        await collection.ReplaceOneAsync(x => x.Name == name, updatedSetting);

    public async Task RemoveAsync(string id) =>
        await collection.DeleteOneAsync(x => x.Id == id);
}