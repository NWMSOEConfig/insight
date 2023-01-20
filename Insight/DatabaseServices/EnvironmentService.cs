using Insight.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Insight.Services;

public class DatabaseEnvironmentService : ServiceParent<DatabaseEnvironment>
{
    public DatabaseEnvironmentService()
    {
         var mongoClient = new MongoClient("mongodb+srv://dbTestUser:friedegg@new-world.tmynaas.mongodb.net/?retryWrites=true&w=majority");

        var mongoDatabase = mongoClient.GetDatabase("Configurations");

        collection = mongoDatabase.GetCollection<DatabaseEnvironment>("Environments");
    }

    public async Task<DatabaseEnvironment?> GetAsync(string id) =>
        await collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<DatabaseEnvironment?> GetNameAsync(string name) =>
        await collection.Find(x => x.Name == name).FirstOrDefaultAsync();
    
    public async Task<DatabaseEnvironment?> GetCategoryAsync(string name) =>
        await collection.Find(x => x.Name == name).FirstOrDefaultAsync();

    public async Task UpdateAsync(string id, DatabaseEnvironment updatedEnvironment) =>
        await collection.ReplaceOneAsync(x => x.Id == id, updatedEnvironment);

    public async Task RemoveAsync(string id) =>
        await collection.DeleteOneAsync(x => x.Id == id);

    public async Task<DatabaseEnvironment> GetByURLAsync(string url) =>
        await collection.Find(x => x.Url == url).FirstOrDefaultAsync();
}