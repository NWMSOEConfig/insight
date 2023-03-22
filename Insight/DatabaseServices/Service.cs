using MongoDB.Driver;

namespace Insight.Services;

public class ServiceParent<T>
{
    protected IMongoCollection<T> collection;

    protected ServiceParent()
    {
    }

    public ServiceParent(IMongoCollection<T> collection)
    {
        this.collection = collection;
    }

    public async Task<List<T>> GetAsync() =>
        await collection.Find(_ => true).ToListAsync();

    public async Task CreateAsync(T newItem) =>
        await collection.InsertOneAsync(newItem);

    public async Task CreateManyAsync(IEnumerable<T> items) =>
        await collection.InsertManyAsync(items);
}
