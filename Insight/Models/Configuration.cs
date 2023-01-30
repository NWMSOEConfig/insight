using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Insight.Models;

[BsonIgnoreExtraElements]
public class DatabaseSetting
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Name { get; set; }= null!;

    public string Category { get; set; } = null!;

    public Parameter[]? Parameters { get; set; }

    public bool Enabled { get; set; }

    public string[] TenantNames { get; set; } = null!;

    public DatabaseEnvironment[] Environments { get; set; } = null!;

    public DatabaseTenant[] Tenants { get; set; } = null!;
}

public class DatabaseTenant {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Name { get; set; } = null!;
    public DatabaseEnvironment[] Environments { get; set; } = null!;
    //public Dictionary<string, DateTime>? EnvironmentLastPulled { get; set; }
    // public string[] Environment { get; set; } = null!;
}

public class QueuedChange {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public DatabaseSetting[] Settings { get; set; } = null!;
    public DatabaseSetting[] OriginalSettings { get; set; } = null!;
    public User User { get; set; } = null!;
    public DatabaseTenant Tenant { get; set; } = null!;
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
    public DateTime Time { get; set; }
    public QueuedChange QueueChange { get; set; } = null!;
}

public class DatabaseEnvironment {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? Name { get; set; }
    public Dictionary<string, DateTime>? EnvironmentLastPulled { get; set; }
    public string? Url { get; set; }

}


