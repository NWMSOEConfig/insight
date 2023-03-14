import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { getTenant, setTenant, Tenant } from './tenant-singleton';
import { environment } from 'src/environments/environment';
import { ApiService } from './api-service/api.service';
import { DatabaseTenant } from 'src/models/databaseTenant';
import { TenantSelectorComponent } from './tenant-selector/tenant-selector.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  toggleDirection: string;

  constructor(private http: HttpClient, private apiService: ApiService) {
    setTenant(getTenant());
    this.toggleDirection =
      localStorage.getItem('toggleDirection') || 'keyboard_arrow_left';
  }

  ngOnInit(): void {}

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
