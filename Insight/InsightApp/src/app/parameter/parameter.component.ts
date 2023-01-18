import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Parameter } from '../../models/parameter';

@Component({
  selector: 'app-parameter',
  templateUrl: './parameter.component.html',
  styleUrls: ['./parameter.component.css'],
})
export class ParameterComponent implements OnInit {
  @Input() parameter!: Parameter;
  @Output() dataChange = new EventEmitter<Parameter>();
  initialValue: any;
  type!: string;
  modified = false;
  inputLabel: string = 'Value';

  createInputLabel(): void {
    const paramName = this.parameter.name.toLowerCase();

    if (paramName.includes('maxcharacterlength')) {
      this.inputLabel = 'Max character length';
    } else if (paramName.includes('enabled')) {
      this.inputLabel = 'Is enabled';
    } else if (paramName.includes('dayssincecompletion')) {
      this.inputLabel = 'Days since completion';
    }
  }

  ngOnInit(): void {
    this.initialValue = this.parameter.value;
    this.createInputLabel();
  }

  change(): void {
    this.modified = this.parameter.value !== this.initialValue;
    this.dataChange.emit(this.parameter);
  }
}
