import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api-service/api.service';

@Component({
  selector: 'app-configuration-page',
  templateUrl: './configuration-page.component.html',
  styleUrls: ['./configuration-page.component.css']
})
export class ConfigurationPageComponent implements OnInit {
  disabled = false;

  constructor(private apiService: ApiService) {
  }

  ngOnInit(): void {
  }

  clickRefresh(): void {
    this.disabled = true;
    this.apiService.postPopulate('https://pauat.newworldnow.com/v7/api/applicationsettings/', 'TODO', 'TODO').subscribe(message => {
      alert(message);
      this.disabled = false;
    });
  }
}
