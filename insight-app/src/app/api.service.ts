import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Setting } from './models/setting';
import { Parameter } from './models/parameter';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  readonly apiURL = 'https://localhost:8000/api';

  constructor(private httpClient: HttpClient) {
  }

  public getCountry() {
    return this.httpClient.get(`${this.apiURL}/todoitems`);
  }

  public getSetting(id: number) {
    return this.httpClient.get<Setting>(`${this.apiURL}/data/setting?id=${id}`);
  }

  public getParameter(id: number) {
    return this.httpClient.get<Parameter>(`${this.apiURL}/data/parameter?id=${id}`);
  }
}
