using Insight.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Insight.Services;

public class DatabaseUserService
{
    private readonly IMongoCollection<User> UserCollection;

    public DatabaseUserService(
        IOptions<DBUserConnection> bookStoreDatabaseUsers)
    {
         var mongoClient = new MongoClient("mongodb+srv://dbTestUser:friedegg@new-world.tmynaas.mongodb.net/?retryWrites=true&w=majority");

        var mongoDatabase = mongoClient.GetDatabase("Configurations");

        UserCollection = mongoDatabase.GetCollection<User>("Users");
    }

    public async Task<List<User>> GetAsync() =>
        await UserCollection.Find(_ => true).ToListAsync();

    public async Task<User?> GetAsync(string id) =>
        await UserCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    
    public async Task<User?> GetCategoryAsync(string name) =>
        await UserCollection.Find(x => x.Name == name).FirstOrDefaultAsync();

    public async Task CreateAsync(User newUser) =>
        await UserCollection.InsertOneAsync(newUser);

    public async Task UpdateAsync(string id, User updatedUser) =>
        await UserCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);

    public async Task RemoveAsync(string id) =>
        await UserCollection.DeleteOneAsync(x => x.Id == id);
}