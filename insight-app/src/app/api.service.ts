import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

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
    return this.httpClient.get(`${this.apiURL}/data/setting?id=${id}`);
  }

  public getParameter(id: number) {
    return this.httpClient.get(`${this.apiURL}/data/parameter?id=${id}`);
  }
}
