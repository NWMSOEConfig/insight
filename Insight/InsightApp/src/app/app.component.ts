import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  toggleDirection: string = 'keyboard_arrow_right';
  tenants: any = [
    { site: 'Wisconsin', environments: ['Test', 'Prod'] },
    { site: 'Minnesota', environments: ['Prod'] },
    { site: 'Indiana', environments: ['Prod'] },
    { site: 'Ohio', environments: ['Prod', 'Test', 'Test2'] },
    { site: 'Michigan', environments: ['Prod'] },
  ];
  selectedTenant: any = { site: null, environment: null };

  constructor(private http: HttpClient) {}

  changeTenant(site: string, environment: string): void {
    this.selectedTenant.site = site;
    this.selectedTenant.environment = environment;
  }

  toggleSidebar(sidebar: any): void {
    sidebar.toggle();
    this.toggleDirection === 'keyboard_arrow_right'
      ? (this.toggleDirection = 'keyboard_arrow_left')
      : (this.toggleDirection = 'keyboard_arrow_right');
  }
}
