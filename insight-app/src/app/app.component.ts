import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'insight-app';
  sites = [
    {name: "Wisconsin"},
    {name: "South Carolina"},
    {name: "North Dakota"}

  ]
}
