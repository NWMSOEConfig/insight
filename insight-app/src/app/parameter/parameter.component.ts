import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ApiService } from '../api.service';

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

  constructor(private apiService: ApiService) {
  }

  ngOnInit(): void {
    this.apiService.getParameter(this.parameterId).subscribe((data: any) => {
      this.param = data;
      this.value = this.param.value;
      this.initialValue = this.param.value;
      this.dataChange.emit({
        name: this.param.name,
        type: this.param.type,
        value: this.value,
      });
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
