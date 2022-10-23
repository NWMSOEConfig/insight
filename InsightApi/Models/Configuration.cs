using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InsightApi.Models;


public class Setting
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Category { get; set; } = null!;

    public string[]? Parameter { get; set; }

    public bool Enabled { get; set; }
    public Tenant[] Tenants { get; set; } = null!;
}

public class Tenant {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Name { get; set; } = null!;
    public string[] Environment { get; set; } = null!;

}

public class QueuedChange {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public Setting[] Settings { get; set; } = null!;
    public Setting[] OriginalSettings { get; set; } = null!;
    public User User { get; set; } = null!;
    public Tenant Tenant { get; set; } = null!;

}

public class User {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Name { get; set; } = null!;
    public int UserId { get; set; }
}

public class Commit {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public int CommitId { get; set; }
    public double Time { get; set; }
    public QueuedChange QueueChange { get; set; } = null!;
}


public class DBSettingConnection
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string SettingsCollectionName { get; set; } = null!;
}

public class DBTenantConnection
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string TenantsCollectionName { get; set; } = null!;
}

public class DBCommmitConnection
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string CommitsCollectionName { get; set; } = null!;
}

public class DBQueuedChangesConnection
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string QueuedChangesCollectionName { get; set; } = null!;
}

public class DBUserConnection
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string UsersCollectionName { get; set; } = null!;
}