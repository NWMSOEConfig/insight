using Insight.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Insight.Services;

public class DatabaseSettingsService
{
    private readonly IMongoCollection<DatabaseSetting> settingsCollection;

    public DatabaseSettingsService()
    {
        var mongoClient = new MongoClient("mongodb+srv://dbTestUser:friedegg@new-world.tmynaas.mongodb.net/?retryWrites=true&w=majority");

        var mongoDatabase = mongoClient.GetDatabase("Configurations");

        settingsCollection = mongoDatabase.GetCollection<DatabaseSetting>("Settings");
    }

    public async Task<List<DatabaseSetting>> GetAsync() =>
        await settingsCollection.Find(_ => true).ToListAsync();

    public async Task<DatabaseSetting?> GetAsync(string id) =>
        await settingsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    
    public async Task<List<DatabaseSetting>> GetEnvironmentAsync(string tenantId)
    {
        return await settingsCollection.Find(x => x.TenantNames.Contains(tenantId)).ToListAsync();
    }

    public async Task CreateAsync(DatabaseSetting newSetting) =>
        await settingsCollection.InsertOneAsync(newSetting);

    public async Task UpdateAsync(string id, DatabaseSetting updatedSetting) =>
        await settingsCollection.ReplaceOneAsync(x => x.Id == id, updatedSetting);

    public async Task RemoveAsync(string id) =>
        await settingsCollection.DeleteOneAsync(x => x.Id == id);
}