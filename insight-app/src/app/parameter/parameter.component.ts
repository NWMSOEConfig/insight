import { Component, Input, OnInit } from '@angular/core';

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
    value: 123,
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
  param: any;

  constructor() {
  }

  ngOnInit(): void {
    this.param = temp_data[this.parameterId];
  }

}
