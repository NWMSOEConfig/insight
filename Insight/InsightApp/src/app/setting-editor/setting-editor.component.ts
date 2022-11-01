import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ApiService } from '../api.service';
import { Parameter } from '../../models/parameter';

@Component({
  selector: 'app-setting-editor',
  templateUrl: './setting-editor.component.html',
  styleUrls: ['./setting-editor.component.css']
})
export class SettingEditorComponent implements OnInit {
  @Input() settingId = 0;
  @Output() cancel = new EventEmitter();
  data: any = {};
  parameterIds: number[] = [];

  constructor(private apiService: ApiService) {
  }

  ngOnInit(): void {
    this.apiService.getSetting(this.settingId).subscribe(setting => {
      this.parameterIds = setting.parameterIds;
    });
  }

  clickCancel(): void {
    this.cancel.emit();
  }

  queue(): void {
    this.apiService.postQueue(this.settingId, Object.values(this.data)).subscribe(message => {
      alert(message);
    });
  }

  dataChange(parameterId: number, newData: Parameter): void {
    this.data[parameterId] = newData;
  }
}
