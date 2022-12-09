using Insight.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Insight.Services;

public class DatabaseUserService : ServiceParent<User>
{
    public DatabaseUserService()
    {
         var mongoClient = new MongoClient("mongodb+srv://dbTestUser:friedegg@new-world.tmynaas.mongodb.net/?retryWrites=true&w=majority");

        var mongoDatabase = mongoClient.GetDatabase("Configurations");

        collection = mongoDatabase.GetCollection<User>("Users");
    }

    public async Task<User?> GetAsync(string id) =>
        await collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    
    public async Task<User?> GetCategoryAsync(string name) =>
        await collection.Find(x => x.Name == name).FirstOrDefaultAsync();

    public async Task UpdateAsync(string id, User updatedUser) =>
        await collection.ReplaceOneAsync(x => x.Id == id, updatedUser);

    public async Task RemoveAsync(string id) =>
        await collection.DeleteOneAsync(x => x.Id == id);
}