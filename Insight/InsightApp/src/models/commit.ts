import { Setting } from './setting';

export interface QueueChange {
  Settings: any;
  OriginalSettings: any;
  User: any;
}

export interface Commit {
  CommitMessage: string;
  ReferenceId: number;
  CommitId: number;
  Time: Date;
  QueueChange: QueueChange;
}
