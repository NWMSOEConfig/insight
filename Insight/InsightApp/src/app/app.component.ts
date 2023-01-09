import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { setTenant } from './tenant-singleton';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  toggleDirection: string;
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
    this.toggleDirection = localStorage.getItem('toggleDirection') || 'keyboard_arrow_left'; 
  }

  changeTenant(site: string, environment: string): void {
    localStorage.setItem('tenant', JSON.stringify(this.selectedTenant));
    setTenant(this.getTenant());
    this.selectedTenant = {};
  }

  /**
   * Get the tenant a client has previously selected to persist data despite browser refreshes
   * @returns client's local storage of Tenant selection (site and environment)
   */
  getTenant(): any {
    const noTenant = JSON.stringify({ site: null, environment: null });
    return JSON.parse(localStorage.getItem('tenant') || noTenant);
  }

  toggleSidebar(sidebar: any): void {
    this.toggleDirection === 'keyboard_arrow_right'
      ? (this.toggleDirection = 'keyboard_arrow_left')
      : (this.toggleDirection = 'keyboard_arrow_right');

    localStorage.setItem('toggleDirection', this.toggleDirection)
  }

  isSidebarToggled(): boolean {
    return this.toggleDirection === 'keyboard_arrow_right' ? false : true;
  }
}
