import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-setting-editor',
  templateUrl: './setting-editor.component.html',
  styleUrls: ['./setting-editor.component.css']
})
export class SettingEditorComponent implements OnInit {
  data = [{}, {}, {}, {}]

  constructor() { }

  ngOnInit(): void {
  }

  queue(): void {
    console.log('queue pressed');
    console.log(this.data);
  }

  dataChange(parameterId: number, newData: {name: string, type: string, value: any}): void {
    this.data[parameterId] = {...this.data, ...newData};
  }
}
