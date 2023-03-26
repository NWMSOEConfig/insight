import { Setting } from "./setting"

export interface QueueChange {
  Settings: Setting[],
  OriginalSettings: Setting[],
  User: any
}

export interface Commit {
  Message: "hiad",
  CommitId: number,
  Time: Date,
  QueueChange: QueueChange[]
}