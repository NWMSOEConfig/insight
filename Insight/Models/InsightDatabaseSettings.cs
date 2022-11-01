namespace Insight.Models;

public class DBSettingConnection
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string SettingsCollectionName { get; set; } = null!;
}
