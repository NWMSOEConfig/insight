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
    { site: 'WI', environments: ['Dev', 'UAT', 'Test', 'Alt', 'Prod'] },
    { site: 'MN', environments: ['Prod'] },
    { site: 'IN', environments: ['Dev', 'Test', 'Prod'] },
    { site: 'OH', environments: ['Test', 'Alt', 'Prod'] },
    { site: 'MI', environments: ['UAT', 'Prod'] },
  ];
  selectedTenant: any = {};

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
