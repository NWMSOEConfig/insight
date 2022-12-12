import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Parameter } from '../../models/parameter';

@Component({
  selector: 'app-parameter',
  templateUrl: './parameter.component.html',
  styleUrls: ['./parameter.component.css']
})
export class ParameterComponent implements OnInit {
  @Input() parameter!: Parameter;
  @Output() dataChange = new EventEmitter<Parameter>();
  initialValue: any;
  type!: string;
  modified = false;

  ngOnInit(): void {
    this.initialValue = this.parameter.value;
    this.type = typeof this.parameter.value;
  }

  change(): void {
    this.modified = this.parameter.value !== this.initialValue;
    this.dataChange.emit(this.parameter);
  }
}
