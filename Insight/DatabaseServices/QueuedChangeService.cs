using Insight.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Insight.Services;

public class DatabaseQueuedChangeService : ServiceParent<QueuedChange>
{

    public DatabaseQueuedChangeService()
    {
        var mongoClient = new MongoClient("mongodb+srv://dbTestUser:friedegg@new-world.tmynaas.mongodb.net/?retryWrites=true&w=majority");

        var mongoDatabase = mongoClient.GetDatabase("Configurations");

        collection = mongoDatabase.GetCollection<QueuedChange>("Queued Changes");
    }

    public async Task<QueuedChange?> GetAsync(string id) =>
        await collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task UpdateAsync(string id, QueuedChange updatedQueuedChange) =>
        await collection.ReplaceOneAsync(x => x.Id == id, updatedQueuedChange);

    public async Task RemoveAsync(string id) =>
        await collection.DeleteOneAsync(x => x.Id == id);
}