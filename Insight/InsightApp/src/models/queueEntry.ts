import { Parameter } from "./parameter";

export interface QueueEntry {
  settingName: string,
  originalParameters: Parameter[],
  newParameters: Parameter[],
  userId: string,
}
