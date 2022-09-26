import { Component, Input, OnInit } from '@angular/core';
import { getSetting } from '../data-service';

@Component({
  selector: 'app-setting-editor',
  templateUrl: './setting-editor.component.html',
  styleUrls: ['./setting-editor.component.css']
})
export class SettingEditorComponent implements OnInit {
  @Input() settingId = 0;
  data: any = {};
  parameterIds: number[] = [];

  constructor() { }

  ngOnInit(): void {
    this.parameterIds = getSetting(this.settingId).parameters;
  }

  queue(): void {
    console.log('queue pressed');
    console.log(this.data);
  }

  dataChange(parameterId: number, newData: {name: string, type: string, value: any}): void {
    this.data[parameterId] = newData;
  }
}
