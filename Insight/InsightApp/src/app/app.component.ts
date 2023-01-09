import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { setTenant } from './tenant-singleton';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  toggleDirection: string = 'keyboard_arrow_left';
  tenants: any = [
    { site: 'WI', environments: ['Dev', 'UAT', 'Test', 'Alt', 'Prod'] },
    { site: 'MN', environments: ['Prod'] },
    { site: 'IN', environments: ['Dev', 'Test', 'Prod'] },
    { site: 'OH', environments: ['Test', 'Alt', 'Prod'] },
    { site: 'MI', environments: ['UAT', 'Prod'] },
  ];
  selectedTenant: any = {};

  constructor(private http: HttpClient) {
    setTenant(this.getTenant());
  }

  changeTenant(site: string, environment: string): void {
    localStorage.setItem('site', site); // Set site data to client's local storage
    localStorage.setItem('environment', environment); // Set environment data to client's local storage
    setTenant(this.getTenant());
    this.selectedTenant = {};
  }

  /**
   * Get the tenant a client has previously selected to persist data despite browser refreshes
   * @returns client's local storage of Tenant selection (site and environment)
   */
  getTenant(): any {
    return {
      site: localStorage.getItem('site'),
      environment: localStorage.getItem('environment'),
    };
  }

  toggleSidebar(sidebar: any): void {
    sidebar.toggle();
    this.toggleDirection === 'keyboard_arrow_right'
      ? (this.toggleDirection = 'keyboard_arrow_left')
      : (this.toggleDirection = 'keyboard_arrow_right');
  }
}
