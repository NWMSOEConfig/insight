import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
/* Models */
import { Setting } from '../models/setting';
import { Parameter } from '../models/parameter';
import { Category } from '../models/category';
import { Subcategory } from '../models/subcategory';
import { Tenant } from '../models/tenant';


@Injectable({
  providedIn: 'root',
})
export class ApiService {
  readonly apiURL = 'https://localhost:8000/api';

  constructor(private httpClient: HttpClient) {}

  public getSetting(id: number) {
    return this.httpClient.get<Setting>(`${this.apiURL}/data/setting?id=${id}`);
  }

  public getParameter(id: number) {
    return this.httpClient.get<Parameter>(
      `${this.apiURL}/data/parameter?id=${id}`
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

  public postQueue(parameters: Parameter[]) {
    return this.httpClient.post(`${this.apiURL}/data/queue`, parameters, {
      responseType: 'text',
    });
  }
}
