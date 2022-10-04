import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
@Injectable({
  providedIn: 'root',
})
export class ApiService {
  readonly apiURL = 'https://localhost:8000/api/';

  constructor(private httpClient: HttpClient) {}

  public getCountry() {
    let headers = new HttpHeaders();
    return this.httpClient.get(this.apiURL + 'todoitems', { headers: headers });
  }
}
