using InsightApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InsightApi.Services;

public class DatabaseSettingsService
{
    private readonly IMongoCollection<Setting> settingsCollection;

    public DatabaseSettingsService(
        IOptions<DBSettingConnection> bookStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            bookStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            bookStoreDatabaseSettings.Value.DatabaseName);

        settingsCollection = mongoDatabase.GetCollection<Setting>(
            bookStoreDatabaseSettings.Value.SettingsCollectionName);
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