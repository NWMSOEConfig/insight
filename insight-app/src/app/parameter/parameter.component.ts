import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

// TODO: fetch parameter schemas and current values from somewhere
const temp_data = [
  {
    name: 'Enabled',
    type: 'boolean',
    value: true,
  },
  {
    name: 'Foo',
    type: 'number',
    value: '123',
  },
  {
    name: 'Bar',
    type: 'text',
    value: 'Text',
  },
  {
    name: 'Baz',
    type: 'email',
    value: 'a@b.com',
  },
];

@Component({
  selector: 'app-parameter',
  templateUrl: './parameter.component.html',
  styleUrls: ['./parameter.component.css']
})
export class ParameterComponent implements OnInit {
  @Input() parameterId = -1;
  @Output() dataChange = new EventEmitter<{name: string, type: string, value: any}>();
  param!: {name: string, type: string, value: any};
  value: any;
  initialValue: any;
  modified = false;

  constructor() {
  }

  ngOnInit(): void {
    this.param = temp_data[this.parameterId];
    this.value = this.param.value;
    this.initialValue = this.param.value;
    this.dataChange.emit({
      name: this.param.name,
      type: this.param.type,
      value: this.value,
    });
  }

  change(): void {
    this.modified = this.value !== this.initialValue;
    this.dataChange.emit({
      name: this.param.name,
      type: this.param.type,
      value: this.value,
    });
  }
}
