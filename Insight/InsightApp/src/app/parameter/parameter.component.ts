import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ApiService } from '../api-service/api.service';
import { Parameter } from '../../models/parameter';

@Component({
  selector: 'app-parameter',
  templateUrl: './parameter.component.html',
  styleUrls: ['./parameter.component.css']
})
export class ParameterComponent implements OnInit {
  @Input() parameterId = -1;
  @Output() dataChange = new EventEmitter<Parameter>();
  param!: Parameter;
  value: any;
  initialValue: any;
  modified = false;
  loaded = false;

  constructor(private apiService: ApiService) {
  }

  ngOnInit(): void {
    this.apiService.getParameter(this.parameterId).subscribe(parameter => {
      this.param = parameter;
      this.value = this.param.value;
      this.initialValue = this.param.value;
      this.loaded = true;
      this.dataChange.emit({
        name: this.param.name,
        value: this.value,
        isBrowserVisible: this.param.isBrowserVisible,
      });
    });
  }

  change(): void {
    this.modified = this.value !== this.initialValue;
    this.dataChange.emit({
      name: this.param.name,
      value: this.value,
      isBrowserVisible: this.param.isBrowserVisible,
    });
  }
}
