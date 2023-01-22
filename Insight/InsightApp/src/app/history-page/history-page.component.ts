import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api-service/api.service';
import { PageEvent } from '@angular/material/paginator';
import { mockCommits } from './mock-commits';

@Component({
  selector: 'app-history-page',
  templateUrl: './history-page.component.html',
  styleUrls: ['./history-page.component.css'],
})
export class HistoryPageComponent implements OnInit {
  mockCommits: any;
  few = 4; // what "Few" means in getFirstFewFromBatch method
  pageIndex = 0;
  pageSize = 5;
  pageSizeOptions = [this.pageSize, 10, 20];
  pageEvent: PageEvent = new PageEvent();

  constructor(private apiService: ApiService) {
    this.mockCommits = mockCommits;
    this.pageEvent = new PageEvent();
  }

  getTimestamp(dateTime: any): Date {
    return new Date(dateTime);
  }

  handlePageEvent(e: PageEvent) {
    this.pageEvent = e;
    this.pageSize = e.pageSize;
    this.pageIndex = e.pageIndex;
  }

  getPage(): any {
    var startIndex = this.pageIndex * this.pageSize;
    return mockCommits.slice(startIndex, startIndex + this.pageSize);
  }

  /**
   * Get the first few (defined above) entries from a list of settings
   * @param batch
   * @returns first few entries
   */
  getFirstFewFromBatch(batch: any): any {
    return batch.slice(0, this.few);
  }

  getRemainingBatchCount(batch: any): any {
    return batch.length - this.getFirstFewFromBatch(batch).length;
  }

  ngOnInit() {}
}
