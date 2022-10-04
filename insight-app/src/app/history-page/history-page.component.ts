import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api.service';

@Component({
  selector: 'app-history-page',
  templateUrl: './history-page.component.html',
  styleUrls: ['./history-page.component.css'],
})


export class HistoryPageComponent implements OnInit {
  countryList: any;

  constructor(private apiService: ApiService) {}

  ngOnInit() {
    this.apiService.getCountry().subscribe((data) => {
      // console.log(data);
      console.log("XD")
      this.countryList = data;
    });
  }
}
