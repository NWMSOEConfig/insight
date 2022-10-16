namespace InsightApi.Models;
public record Category(int Id, string Name, IList<int> Subcategories);