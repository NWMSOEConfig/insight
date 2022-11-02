using Insight.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Insight.Services;

public class DatabaseTenantService
{
    private readonly IMongoCollection<DatabaseTenant> TenantsCollection;

    public DatabaseTenantService()
    {
         var mongoClient = new MongoClient("mongodb+srv://dbTestUser:friedegg@new-world.tmynaas.mongodb.net/?retryWrites=true&w=majority");

        var mongoDatabase = mongoClient.GetDatabase("Configurations");

        TenantsCollection = mongoDatabase.GetCollection<DatabaseTenant>("DatabaseTenants");
    }

    public async Task<List<DatabaseTenant>> GetAsync() =>
        await TenantsCollection.Find(_ => true).ToListAsync();

    public async Task<DatabaseTenant?> GetAsync(string id) =>
        await TenantsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    
    public async Task<DatabaseTenant?> GetCategoryAsync(string name) =>
        await TenantsCollection.Find(x => x.Name == name).FirstOrDefaultAsync();

    public async Task CreateAsync(DatabaseTenant newDatabaseTenant) =>
        await TenantsCollection.InsertOneAsync(newDatabaseTenant);

    public async Task UpdateAsync(string id, DatabaseTenant updatedDatabaseTenant) =>
        await TenantsCollection.ReplaceOneAsync(x => x.Id == id, updatedDatabaseTenant);

    public async Task RemoveAsync(string id) =>
        await TenantsCollection.DeleteOneAsync(x => x.Id == id);
}