namespace Insight.Models;

public record QueueEntry(string SettingName, IList<Parameter> OriginalParameters, IList<Parameter> ChangedParameters, string Queuer);
