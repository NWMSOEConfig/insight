using InsightApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InsightApi.Services;

public class DatabaseSettingsService
{
    private readonly IMongoCollection<Setting> settingsCollection;

    public DatabaseSettingsService(
        IOptions<DBSettingConnection> settingsDatabaseSettings)
    {
        var mongoClient = new MongoClient("mongodb+srv://dbTestUser:friedegg@new-world.tmynaas.mongodb.net/?retryWrites=true&w=majority");

        var mongoDatabase = mongoClient.GetDatabase("Configurations");

        settingsCollection = mongoDatabase.GetCollection<Setting>("Settings");
    }

    public async Task<List<Setting>> GetAsync() =>
        await settingsCollection.Find(_ => true).ToListAsync();

    public async Task<Setting?> GetAsync(string id) =>
        await settingsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    
    public async Task<Setting?> GetCategoryAsync(string category) =>
        await settingsCollection.Find(x => x.Category == category).FirstOrDefaultAsync();

    public async Task CreateAsync(Setting newSetting) =>
        await settingsCollection.InsertOneAsync(newSetting);

    public async Task UpdateAsync(string id, Setting updatedSetting) =>
        await settingsCollection.ReplaceOneAsync(x => x.Id == id, updatedSetting);

    public async Task RemoveAsync(string id) =>
        await settingsCollection.DeleteOneAsync(x => x.Id == id);
}