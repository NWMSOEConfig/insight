import { Setting } from "./setting"

export interface QueueChange {
  Settings: Setting[],
  OriginalSettings: Setting[]
}

export interface Commit {
  CommitId: number,
  Time: Date,
  QueueChange: QueueChange[]
}