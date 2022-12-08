using Insight.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Insight.Services;

public class DatabaseCommitService : ServiceParent<Commit>
{

    public DatabaseCommitService()
    {
        var mongoClient = new MongoClient("mongodb+srv://dbTestUser:friedegg@new-world.tmynaas.mongodb.net/?retryWrites=true&w=majority");

        var mongoDatabase = mongoClient.GetDatabase("Configurations");

        collection = mongoDatabase.GetCollection<Commit>("Commits");
    }

    public async Task<Commit?> GetAsync(string id) =>
        await collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<Commit?> GetTimeAsync(int time) =>
        await collection.Find(x => x.Time == time).FirstOrDefaultAsync();

    public async Task UpdateAsync(string id, Commit updatedCommit) =>
        await collection.ReplaceOneAsync(x => x.Id == id, updatedCommit);

    public async Task RemoveAsync(string id) =>
        await collection.DeleteOneAsync(x => x.Id == id);
}