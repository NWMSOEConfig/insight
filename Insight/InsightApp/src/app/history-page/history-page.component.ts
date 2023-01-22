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
  commits: any;
  filteredCommits: any;
  few = 4; // what "Few" means in getFirstFewFromBatch method
  pageIndex = 0;
  pageSize = 10;
  pageSizeOptions = [this.pageSize, this.pageSize * 2, this.pageSize * 5];
  pageEvent: PageEvent = new PageEvent();
  minDate = new Date();
  maxDate = new Date(); // max date is current day
  selectedMinDate = new Date();
  selectedMaxDate = new Date();

  constructor(private apiService: ApiService) {
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

  resetTime(date: Date): Date {
    date.setHours(0);
    date.setMinutes(0);
    date.setSeconds(0);
    date.setMilliseconds(0);
    return date;
  }

  /**
   * paginator to get commits per page
   */
  getPage(): any {
    var startIndex = this.pageIndex * this.pageSize;
    this.filteredCommits = this.commits
      .filter(
        (c: { timestamp: number }) =>
          this.resetTime(new Date(c.timestamp)) <=
          this.resetTime(this.selectedMaxDate)
      )
      .filter(
        (c: { timestamp: number }) =>
          this.resetTime(new Date(c.timestamp)) >=
          this.resetTime(this.selectedMinDate)
      );

    return this.filteredCommits.slice(startIndex, startIndex + this.pageSize);
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

  ngOnInit() {
    this.commits = mockCommits.sort(
      (a, b) =>
        new Date(b.timestamp).getTime() - new Date(a.timestamp).getTime()
    ); // Get data from mock commits and sort by timestamp (oldest to newest)

    this.filteredCommits = this.commits;

    this.commits.forEach((c: { timestamp: string }) => {
      var timestamp = new Date(c.timestamp);
      this.minDate < timestamp ? null : (this.minDate = timestamp);
    }); // Set the minimum selectable date on date picker

    this.commits.forEach((c: { timestamp: string }) => {
      var timestamp = new Date(c.timestamp);
      this.maxDate > timestamp ? null : (this.maxDate = timestamp);
    }); // Set the maximum selectable date on date picker

    this.selectedMinDate = this.minDate;
    this.selectedMaxDate = this.maxDate;
  }
}
