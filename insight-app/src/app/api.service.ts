import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
@Injectable({
  providedIn: 'root',
})
export class ApiService {
  readonly apiURL = 'https://localhost:8000/api/';

  constructor(private httpClient: HttpClient) {}

  public getCountry() {
    console.log('GET');
    let headers = new HttpHeaders();
    headers = headers.set('Access-Control-Allow-Origin', '*');
    // headers = headers.set('content-type', 'application/json');

    return this.httpClient.get(this.apiURL + 'todoitems', { headers: headers });
  }
}
