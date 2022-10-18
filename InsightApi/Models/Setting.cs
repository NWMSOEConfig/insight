namespace InsightApi.Models;

public record Setting(int Id, string Name, IList<int> ParameterIds);
