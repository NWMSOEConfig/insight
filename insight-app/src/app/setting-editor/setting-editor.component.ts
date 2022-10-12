import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ApiService } from '../api.service';

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
    this.apiService.getSetting(this.settingId).subscribe((data: any) => {
      this.parameterIds = data;
    });
  }

  clickCancel(): void {
    this.cancel.emit();
  }

  queue(): void {
    console.log('queue pressed');
    console.log(this.data);
  }

  dataChange(parameterId: number, newData: {name: string, type: string, value: any}): void {
    this.data[parameterId] = newData;
  }
}
