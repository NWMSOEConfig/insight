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

    public DatabaseQueuedChangeService(IMongoCollection<QueuedChange> collection)
    {
        this.collection = collection;
    }

    public virtual async Task<QueuedChange?> GetAsync(string userName, string tenantName, string environmentName) =>
        await collection.Find(x => x.User.Name == userName
                                   && x.Tenant.Name == tenantName
                                   && x.Tenant.Environments.Any(env => env.Name == environmentName))
                        .FirstOrDefaultAsync();

    public async Task UpdateAsync(string id, QueuedChange updatedQueuedChange) =>
        await collection.ReplaceOneAsync(x => x.Id == id, updatedQueuedChange);

    public async Task RemoveAsync(string id) =>
        await collection.DeleteOneAsync(x => x.Id == id);

    public async Task CreateOrUpdateAsync(QueuedChange change)
    {
        var existing = await collection.Find(x => x.User == change.User
                                             && x.Tenant == change.Tenant
                                             && x.Tenant.Environments == change.Tenant.Environments)
                                       .FirstOrDefaultAsync() is not null;

        if (existing)
            await collection.ReplaceOneAsync(x => x.User == change.User
                                             && x.Tenant == change.Tenant
                                             && x.Tenant.Environments == change.Tenant.Environments, change);
        else
            await collection.InsertOneAsync(change);
    }

    public async Task DeleteAllAsync() {
        await collection.DeleteManyAsync(_ => true);
    }

    public async Task DeleteSingleSetting(string settingName)
    {
        await collection.DeleteOneAsync(x => x.Settings[0].newSetting.Name == settingName);
    }
}
