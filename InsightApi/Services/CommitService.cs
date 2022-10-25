using InsightApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InsightApi.Services;

public class DatabaseCommitService
{
    private readonly IMongoCollection<Commit> CommitsCollection;

    public DatabaseCommitService(
        IOptions<DBCommmitConnection> InsightDatabaseCommits)
    {
        var mongoClient = new MongoClient("mongodb+srv://dbTestUser:friedegg@new-world.tmynaas.mongodb.net/?retryWrites=true&w=majority");

        var mongoDatabase = mongoClient.GetDatabase("Configurations");

        CommitsCollection = mongoDatabase.GetCollection<Commit>("Commits");
    }

    public async Task<List<Commit>> GetAsync() =>
        await CommitsCollection.Find(_ => true).ToListAsync();

    public async Task<Commit?> GetAsync(string id) =>
        await CommitsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<Commit?> GetTimeAsync(int time) =>
        await CommitsCollection.Find(x => x.Time == time).FirstOrDefaultAsync();

    public async Task CreateAsync(Commit newCommit) =>
        await CommitsCollection.InsertOneAsync(newCommit);

    public async Task UpdateAsync(string id, Commit updatedCommit) =>
        await CommitsCollection.ReplaceOneAsync(x => x.Id == id, updatedCommit);

    public async Task RemoveAsync(string id) =>
        await CommitsCollection.DeleteOneAsync(x => x.Id == id);
}