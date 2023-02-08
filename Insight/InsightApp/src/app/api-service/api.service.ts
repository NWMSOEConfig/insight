import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
/* Models */
import { Setting } from '../../models/setting';
import { Category } from '../../models/category';
import { Subcategory } from '../../models/subcategory';
import { Tenant } from '../../models/tenant';
import { getTenant } from '../tenant-singleton';


interface Settings {};

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  public readonly apiURL = 'https://localhost:8000/api';

  constructor(private httpClient: HttpClient) {}

  public getSetting(name: string) {
    const context = getTenant();
    return this.httpClient.get<Setting>(
      `${this.apiURL}/data/livesetting?settingName=${name}&tenantName=${context.site}&environmentName=${context.environment}`
    );
  }

  public getAllSettings() {
    const context = getTenant();
    return this.httpClient.get<Settings>(`${this.apiURL}/data/livesettings`);
  }

  public getCategory(id: number) {
    return this.httpClient.get<Category>(
      `${this.apiURL}/data/category?id=${id}`
    );
  }

  public getSubcategory(id: number) {
    return this.httpClient.get<Subcategory>(
      `${this.apiURL}/data/subcategory?id=${id}`
    );
  }

  public getTenant(id: number) {
    return this.httpClient.get<Tenant>(`${this.apiURL}/data/tenant?id=${id}`);
  }

  public postQueue(setting: Setting) {
    const context = getTenant();
    return this.httpClient.post(`${this.apiURL}/data/queue?tenantName=${context.site}&environmentName=${context.environment}`, setting, {
      responseType: 'text',
    });
  }

  public postPopulate(url: string, tenant: string, environment: string) {
    return this.httpClient.post(`${this.apiURL}/data/populate?tenant=${tenant}&environment=${environment}`, JSON.stringify(url), {
      headers: {
        'Content-Type': 'application/json',
      },
    });
  }
}
