namespace Insight.Models;

public record Tenant(int Id, string Name, IList<int> CategoryIds);
