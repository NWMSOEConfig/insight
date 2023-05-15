import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ApiService } from '../api-service/api.service';
import { Parameter } from '../../models/parameter';
import { Setting } from 'src/models/setting';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Commit } from '../../models/commit';

@Component({
  selector: 'app-setting-editor',
  templateUrl: './setting-editor.component.html',
  styleUrls: ['./setting-editor.component.css'],
})
export class SettingEditorComponent implements OnInit {
  @Input() settingName = '';
  @Output() cancel = new EventEmitter();
  @Input() numberOfSettingsToDisplay: any = 4; // Have 4 settings deplayed by default
  setting: Setting | null = null;
  commits: Commit[] = [];

  constructor(private apiService: ApiService, public snackBar: MatSnackBar) {}

  ngOnInit(): void {
    this.apiService.getSetting(this.settingName).subscribe((setting) => {
      this.setting = setting;
    });

    this.apiService.getCommitsBySetting(this.settingName).subscribe((data: Commit[]) => {
      this.commits = data;
    });
  }
  /**
   * Get the first few (defined above) entries from a list of settings
   * @param batch
   * @returns first few entries
   */
  getFirstFewFromBatch(batch: any): any {
    if (batch.length > this.numberOfSettingsToDisplay) {
      return batch.slice(0, this.numberOfSettingsToDisplay);
    } else {
      return batch;
    }
  }

  getRemainingBatchCount(batch: any): any {
    return batch.length - this.getFirstFewFromBatch(batch).length;
  }

  getTimestamp(dateTime: any): Date {
    return new Date(dateTime);
  }

  clickCancel(): void {
    this.cancel.emit();
  }

  queue(): void {
    this.apiService.postQueue(this.setting!).subscribe((message) => {
      this.snackBar.open(message, "Dismiss", {
        duration: 5000
      });
    });
  }

  /**
   * Update the current setting with a parameter's new value.
   * @param newParameter the parameter with the new value
   */
  dataChange(newParameter: Parameter): void {
    for (const parameter of this.setting!.parameters ?? []) {
      if (parameter.name === newParameter.name) {
        parameter.value = newParameter.value;
        break;
      }
    }
  }
}
