import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { setTenant } from './tenant-singleton';
import { environment } from 'src/environments/environment';
import { ApiService } from './api-service/api.service';
import { DatabaseTenant } from 'src/models/databaseTenant';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  toggleDirection: string;
  tenants!: DatabaseTenant[];
  selectedTenant: any = {};

  constructor(private http: HttpClient, private apiService: ApiService) {
    setTenant(this.getTenant());
    this.toggleDirection =
      localStorage.getItem('toggleDirection') || 'keyboard_arrow_left';
  }

  ngOnInit(): void {
    this.apiService.getAllTenants().subscribe((tenants) => {
      this.tenants = tenants;
    });
  }

  changeTenant(site: string, environment: string): void {
    this.selectedTenant = { site: site, environment: environment };
    localStorage.setItem('tenant', JSON.stringify(this.selectedTenant));
    setTenant(this.getTenant());
    this.selectedTenant = {};
    window.location.reload(); // 
  }

  /**
   * Get the tenant a client has previously selected to persist data despite browser refreshes
   * @returns client's local storage of Tenant selection (site and environment)
   */
  getTenant(): any {
    const noTenant = JSON.stringify({ site: null, environment: null });
    return JSON.parse(localStorage.getItem('tenant') || noTenant);
  }

  toggleSidebar(): void {
    this.toggleDirection === 'keyboard_arrow_right'
      ? (this.toggleDirection = 'keyboard_arrow_left')
      : (this.toggleDirection = 'keyboard_arrow_right');

    localStorage.setItem('toggleDirection', this.toggleDirection);
  }

  isSidebarToggled(): boolean {
    return this.toggleDirection === 'keyboard_arrow_right' ? false : true;
  }
}
