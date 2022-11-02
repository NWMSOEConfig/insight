namespace Insight.Models;

public record Setting(int Id, string Name, IList<int> ParameterIds);
