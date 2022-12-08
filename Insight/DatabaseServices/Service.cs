using Insight.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Insight.Services;

public class ServiceParent<T> {
    protected IMongoCollection<T> collection;

    public async Task<List<T>> GetAsync() =>
        await collection.Find(_ => true).ToListAsync();

    public async Task CreateAsync(T newSetting) =>
        await collection.InsertOneAsync(newSetting);
}