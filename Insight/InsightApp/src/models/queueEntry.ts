import { Setting } from "./setting";

export interface Change {
  oldSetting: Setting;
  newSetting: Setting;
}

export interface QueueEntry {
  settings: Change[];
}
