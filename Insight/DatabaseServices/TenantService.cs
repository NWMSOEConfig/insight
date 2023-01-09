using Insight.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Insight.Services;

public class DatabaseTenantService : ServiceParent<DatabaseTenant>
{
    public DatabaseTenantService()
    {
        var mongoClient = new MongoClient("mongodb+srv://dbTestUser:friedegg@new-world.tmynaas.mongodb.net/?retryWrites=true&w=majority");

        var mongoDatabase = mongoClient.GetDatabase("Configurations");

        collection = mongoDatabase.GetCollection<DatabaseTenant>("Tenants");
    }

    public async Task<DatabaseTenant?> GetAsync(string id) =>
        await collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<DatabaseTenant?> GetByNameAsync(string name) =>
        await collection.Find(x => x.Name == name).FirstOrDefaultAsync();

    public async Task<DatabaseTenant?> GetCategoryAsync(string name) =>
        await collection.Find(x => x.Name == name).FirstOrDefaultAsync();

    public async Task UpdateAsync(string id, DatabaseTenant updatedDatabaseTenant) =>
        await collection.ReplaceOneAsync(x => x.Id == id, updatedDatabaseTenant);

    public async Task UpdateByNameAsync(string name, DatabaseTenant updatedDatabaseTenant) =>
        await collection.ReplaceOneAsync(x => x.Name == name, updatedDatabaseTenant);

    public async Task RemoveAsync(string id) =>
        await collection.DeleteOneAsync(x => x.Id == id);
}