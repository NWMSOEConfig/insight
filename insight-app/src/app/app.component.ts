
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'insight-app';
  states: string[] = ['Wisconsin', 'Minnesota', 'Indiana', 'Ohio', 'Michigan'];
  toggle_direction: string = '>';
  selected = "";

  toggle_sidebar(sidebar: any): void {
    sidebar.toggle();
    this.toggle_direction === '>'
      ? (this.toggle_direction = '<')
      : (this.toggle_direction = '>');
  }

  changeSite(selector: string): void {
    this.selected = selector;
  }

  getSite(): string {
    return this.selected;
  }
}


