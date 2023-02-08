import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ApiService } from '../api-service/api.service';
import { Parameter } from '../../models/parameter';
import { Setting } from 'src/models/setting';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-setting-editor',
  templateUrl: './setting-editor.component.html',
  styleUrls: ['./setting-editor.component.css'],
})
export class SettingEditorComponent implements OnInit {
  @Input() settingName = '';
  @Output() cancel = new EventEmitter();
  setting: Setting | null = null;

  constructor(private apiService: ApiService, public snackBar: MatSnackBar) {}

  ngOnInit(): void {
    this.apiService.getSetting(this.settingName).subscribe((setting) => {
      this.setting = setting;
    });
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
