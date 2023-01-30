import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
/* Models */
import { Setting } from '../../models/setting';
import { Category } from '../../models/category';
import { Subcategory } from '../../models/subcategory';
import { Tenant } from '../../models/tenant';
import { QueueEntry } from '../../models/queueEntry';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  public readonly apiURL = 'https://localhost:8000/api';

  constructor(private httpClient: HttpClient) {}

  public getSetting(name: string) {
    return this.httpClient.get<Setting>(
      `${this.apiURL}/data/livesetting?name=${name}`
    );
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
    return this.httpClient.post(`${this.apiURL}/data/queue`, setting, {
      responseType: 'text',
    });
  }

  public getQueue(tenant: string, environment: string) {
    return this.httpClient.get<QueueEntry[]>(`${this.apiURL}/data/queue`);
  }

  public postPopulate(url: string, tenant: string, environment: string) {
    return this.httpClient.post(`${this.apiURL}/data/populate?tenant=${tenant}&environment=${environment}`, JSON.stringify(url), {
      headers: {
        'Content-Type': 'application/json',
      },
    });
  }
}
