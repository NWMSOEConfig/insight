import { Setting } from './setting';

export interface QueueChange {
  Settings: any;
  OriginalSettings: any;
  User: any;
}

export interface Commit {
  Message: 'hiad';
  CommitId: number;
  Time: Date;
  QueueChange: QueueChange;
}
