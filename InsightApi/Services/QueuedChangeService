using InsightApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InsightApi.Services;

public class DatabaseQueuedChangeService
{
    private readonly IMongoCollection<QueuedChange> QueuedChangesCollection;

    public DatabaseQueuedChangeService(
        IOptions<DBQueuedChangesConnection> bookStoreDatabaseQueuedChanges)
    {
        var mongoClient = new MongoClient(
            bookStoreDatabaseQueuedChanges.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            bookStoreDatabaseQueuedChanges.Value.DatabaseName);

        QueuedChangesCollection = mongoDatabase.GetCollection<QueuedChange>(
            bookStoreDatabaseQueuedChanges.Value.QueuedChangesCollectionName);
    }

    public async Task<List<QueuedChange>> GetAsync() =>
        await QueuedChangesCollection.Find(_ => true).ToListAsync();

    public async Task<QueuedChange?> GetAsync(string id) =>
        await QueuedChangesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(QueuedChange newQueuedChange) =>
        await QueuedChangesCollection.InsertOneAsync(newQueuedChange);

    public async Task UpdateAsync(string id, QueuedChange updatedQueuedChange) =>
        await QueuedChangesCollection.ReplaceOneAsync(x => x.Id == id, updatedQueuedChange);

    public async Task RemoveAsync(string id) =>
        await QueuedChangesCollection.DeleteOneAsync(x => x.Id == id);
}