using InsightApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InsightApi.Services;

public class DatabaseUserService
{
    private readonly IMongoCollection<User> UserCollection;

    public DatabaseUserService(
        IOptions<DBUserConnection> bookStoreDatabaseUsers)
    {
        var mongoClient = new MongoClient(
            bookStoreDatabaseUsers.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            bookStoreDatabaseUsers.Value.DatabaseName);

        UserCollection = mongoDatabase.GetCollection<User>(
            bookStoreDatabaseUsers.Value.UsersCollectionName);
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