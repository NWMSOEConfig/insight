namespace Insight.Models;

public record Category(int Id, string Name, IList<int> SubcategoryIds);
