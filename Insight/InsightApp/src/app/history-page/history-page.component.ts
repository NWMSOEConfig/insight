import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api-service/api.service';
import { PageEvent } from '@angular/material/paginator';
import { mockCommits } from './mock-commits';
import { HistoryCardComponent } from '../history-card/history-card.component';
import { Commit, QueueChange } from 'src/models/commit';

@Component({
  selector: 'app-history-page',
  templateUrl: './history-page.component.html',
  styleUrls: ['./history-page.component.css'],
})
export class HistoryPageComponent implements OnInit {
  commits: Commit[] = [];
  filteredCommits: Commit[] = [];
  pageIndex = 0;
  pageSize = 10;
  pageSizeOptions = [
    this.pageSize,
    this.pageSize * 2,
    this.pageSize * 5,
    this.pageSize * 10,
  ];
  pageEvent: PageEvent = new PageEvent();
  minDate = new Date();
  maxDate = new Date(); // max date is current day
  selectedMinDate = new Date();
  selectedMaxDate = new Date();
  searchId: string = '';
  searchUser: string = '';
  searchSetting: string = '';

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

  clearFilters(): void {
    this.selectedMinDate = new Date(this.minDate);
    this.selectedMaxDate = new Date(this.maxDate);
    this.filteredCommits = this.commits;
  }

  getsCommitsByFilter(): void {
    this.filteredCommits = this.commits
      .filter(
        (c: { Time: Date }) =>
          this.resetTime(new Date(c.Time)) <=
          this.resetTime(this.selectedMaxDate)
      ) // Filter lower half of date range
      .filter(
        (c: { Time: Date }) =>
          this.resetTime(new Date(c.Time)) >=
          this.resetTime(this.selectedMinDate)
      ) // Filter upper half of date range
      .filter((c: { CommitId: number }) =>
        c.CommitId.toString()
          .toLowerCase()
          .includes(this.searchId.toLowerCase())
      ) // Filter id
      .filter((c: { QueueChange: QueueChange }) =>
        c.QueueChange.User.Name.toLowerCase().includes(
          this.searchUser.toLowerCase()
        )
      ); // Filter username

    var filteredCommitsBySetting: Commit[] = [];

    for (let i = 0; i < this.filteredCommits.length; i++) {
      var batch = this.filteredCommits.at(i)?.QueueChange.Settings;
      var filter = false;
      for (let j = 0; j < batch.length; j++) {
        var setting = batch.at(j);
        if (
          setting.oldSetting.Name.toLowerCase().startsWith(
            this.searchSetting.toLowerCase()
          )
        ) {
          filter = true;
        }
      }
      if (filter) {
        filteredCommitsBySetting.push(this.filteredCommits.at(i)!);
      }
    }

    this.filteredCommits = filteredCommitsBySetting;
  }

  /**
   * paginator to get commits per page
   */
  getPage(): any {
    var startIndex = this.pageIndex * this.pageSize;

    if (this.filteredCommits.length <= startIndex) {
      this.pageIndex = 0;
      startIndex = 0;
    }

    return this.filteredCommits.slice(startIndex, startIndex + this.pageSize);
  }

  ngOnInit() {
    this.apiService.getAllCommits().subscribe((data: Commit[]) => {
      this.commits = data;

      this.commits = data.sort(
        (a, b) => new Date(b.Time).getTime() - new Date(a.Time).getTime()
      ); // Get data from mock commits and sort by timestamp (oldest to newest)

      this.filteredCommits = this.commits;

      this.commits.forEach((c: { Time: Date }) => {
        var timestamp = new Date(c.Time);
        this.minDate < timestamp ? null : (this.minDate = timestamp);
      }); // Set the minimum selectable date on date picker

      this.commits.forEach((c: { Time: Date }) => {
        var timestamp = new Date(c.Time);
        this.maxDate > timestamp ? null : (this.maxDate = timestamp);
      }); // Set the maximum selectable date on date picker

      this.selectedMinDate = this.minDate;
      this.selectedMaxDate = this.maxDate;
    });
  }
}
