namespace Insight.Models;

public record QueueEntry(int SettingId, IList<Parameter> OriginalParameters, IList<Parameter> ChangedParameters, string Queuer);
