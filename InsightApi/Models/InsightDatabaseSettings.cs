namespace InsightApi.Models;

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