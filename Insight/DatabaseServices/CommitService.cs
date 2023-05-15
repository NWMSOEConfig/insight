using Insight.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Insight.Services;

public class DatabaseCommitService : ServiceParent<Commit>
{

    public DatabaseCommitService()
    {
        var mongoClient = new MongoClient("mongodb+srv://dbTestUser:friedegg@new-world.tmynaas.mongodb.net/?retryWrites=true&w=majority");

        var mongoDatabase = mongoClient.GetDatabase("Configurations");

        collection = mongoDatabase.GetCollection<Commit>("Commits");
    }

    public async Task<Commit?> GetAsync(string id) =>
        await collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<Commit?> GetTimeAsync(DateTime time) =>
        await collection.Find(x => x.Time == time).FirstOrDefaultAsync();

    public async Task UpdateAsync(string id, Commit updatedCommit) =>
        await collection.ReplaceOneAsync(x => x.Id == id, updatedCommit);

    public async Task RemoveAsync(string id) =>
        await collection.DeleteOneAsync(x => x.Id == id);

    public async Task<List<Commit>> GetCommitsAsync(string tenantName, string environmentName)
    {
        List<Commit> commits = new List<Commit>();
        List<Commit> matchedCommits = new List<Commit>();

        // get all commits
        commits = await GetAsync();

        commits.ForEach(commit =>
        {
            if (commit.QueueChange != null)
            {
                QueuedChange queuedChange = commit.QueueChange;

                if (queuedChange.Tenant.Name == tenantName && queuedChange.Tenant.Environments.Any(env => env.Name == environmentName))
                    matchedCommits.Add(commit);
            }

        });

        matchedCommits.OrderBy(commit => commit.CommitId).ToList();
        return matchedCommits;
    }

    public async Task<List<Commit>> GetCommitsBySettingAsync(string tenantName, string environmentName, string settingName)
    {
        List<Commit> commits = new List<Commit>();
        List<Commit> matchedCommits = new List<Commit>();

        // get all commits
        commits = await GetAsync();

        commits.ForEach(commit =>
        {
            if (commit.QueueChange != null)
            {
                QueuedChange queuedChange = commit.QueueChange;

                commit.QueueChange.Settings.ForEach((setting) =>
                {
                    if (setting.oldSetting.Name == settingName &&
                        queuedChange.Tenant.Name == tenantName &&
                        queuedChange.Tenant.Environments.Any(env => env.Name == environmentName))
                    {
                        matchedCommits.Add(commit);
                    }

                });

            }

        });

        matchedCommits.OrderBy(commit => commit.CommitId).ToList();
        return matchedCommits;
    }
    public async Task<Commit?> GetCommitAsync(string tenantName, string environmentName, int id)
    {
        List<Commit> commits = await GetAsync();
        return commits.Find(x => x.CommitId == id && x.QueueChange.Tenant.Name == tenantName && x.QueueChange.Tenant.Environments.Any(env => env.Name == environmentName));
    }
}