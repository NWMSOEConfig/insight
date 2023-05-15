import { Component, OnInit } from '@angular/core';
import { DatabaseTenant } from 'src/models/databaseTenant';
import { ApiService } from '../api-service/api.service';
import { getTenant, setTenant, Tenant } from '../tenant-singleton';

@Component({
  selector: 'app-tenant-selector',
  templateUrl: './tenant-selector.component.html',
  styleUrls: ['./tenant-selector.component.css'],
})
export class TenantSelectorComponent implements OnInit {
  tenants: DatabaseTenant[] = [];
  selectedTenant: any = {};

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.apiService.getAllTenants().subscribe((tenants) => {
      tenants.forEach((tenant) => {
        if (tenant.Name != null) {
          this.tenants.push(tenant);
        }
      })
      this.tenants = this.tenants.sort((a: any, b: any) =>
        a.Name.localeCompare(b.Name)
      );
    });
  }

  getTenant(): Tenant {
    return getTenant();
  }

  changeTenant(site: string, environment: string): void {
    this.selectedTenant = { site: site, environment: environment };
    localStorage.setItem('tenant', JSON.stringify(this.selectedTenant));
    setTenant(getTenant());
    this.selectedTenant = {};
    window.location.reload(); //
  }
}
