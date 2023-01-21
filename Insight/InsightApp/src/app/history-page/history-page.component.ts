import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api-service/api.service';

@Component({
  selector: 'app-history-page',
  templateUrl: './history-page.component.html',
  styleUrls: ['./history-page.component.css'],
})
export class HistoryPageComponent implements OnInit {
  mockCommits: any = [
    {
      id: 'A92CE2L',
      timestamp: '2023-01-09T01:56:47.909Z',
      message: 'Modified all these settings because they were wrong before',
      user: 'Kaif',
      batch: [
        {
          settingName: '',
          oldValue: '',
          newValue: '',
        },
      ],
    },
    {
      id: 'B93ASD3',
      timestamp: '2023-01-13T09:12:03.909Z',
      message: 'Creating settings for a new client in California',
      user: 'Guy',
      batch: [
        {
          settingName: '',
          oldValue: '',
          newValue: '',
        },
      ],
    },
  ];

  getTimestamp(dateTime: any): Date {
    return new Date(dateTime);
  }

  constructor(private apiService: ApiService) {}

  ngOnInit() {}
}
