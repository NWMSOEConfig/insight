namespace InsightApi.Models;
public record Subcategory(int Id, string Name, IList<int> SettingIds);