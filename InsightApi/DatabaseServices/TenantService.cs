using InsightApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InsightApi.Services;

public class DatabaseTenantService
{
    private readonly IMongoCollection<Tenant> TenantsCollection;

    public DatabaseTenantService(
        IOptions<DBTenantConnection> insightDatabaseTenants)
    {
         var mongoClient = new MongoClient("mongodb+srv://dbTestUser:friedegg@new-world.tmynaas.mongodb.net/?retryWrites=true&w=majority");

        var mongoDatabase = mongoClient.GetDatabase("Configurations");

        TenantsCollection = mongoDatabase.GetCollection<Tenant>("Tenants");
    }

    public async Task<List<Tenant>> GetAsync() =>
        await TenantsCollection.Find(_ => true).ToListAsync();

    public async Task<Tenant?> GetAsync(string id) =>
        await TenantsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    
    public async Task<Tenant?> GetCategoryAsync(string name) =>
        await TenantsCollection.Find(x => x.Name == name).FirstOrDefaultAsync();

    public async Task CreateAsync(Tenant newTenant) =>
        await TenantsCollection.InsertOneAsync(newTenant);

    public async Task UpdateAsync(string id, Tenant updatedTenant) =>
        await TenantsCollection.ReplaceOneAsync(x => x.Id == id, updatedTenant);

    public async Task RemoveAsync(string id) =>
        await TenantsCollection.DeleteOneAsync(x => x.Id == id);
}