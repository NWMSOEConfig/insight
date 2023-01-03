import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  tenants: any = [
    { site: 'Wisconsin', environments: [] },
    { site: 'Minnesota', environments: [] },
    { site: 'Indiana', environments: [] },
    { site: 'Ohio', environments: [] },
    { site: 'Michigan', environments: [] },
  ];
  toggle_direction: string = 'keyboard_arrow_right';
  selected = '';

  constructor(private http: HttpClient) {}

  toggle_sidebar(sidebar: any): void {
    sidebar.toggle();
    this.toggle_direction === 'keyboard_arrow_right'
      ? (this.toggle_direction = 'keyboard_arrow_left')
      : (this.toggle_direction = 'keyboard_arrow_right');
  }

  changeSite(selector: string): void {
    this.selected = selector;
  }

  getSite(): string {
    return this.selected;
  }
}
