import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Commit } from 'src/models/commit';
import { ApiService } from '../api-service/api.service';
import { mockCommits } from '../history-page/mock-commits';

@Component({
  selector: 'app-commit-page',
  templateUrl: './commit-page.component.html',
  styleUrls: ['./commit-page.component.css'],
})
export class CommitPageComponent implements OnInit {
  commit!: Commit;

  constructor(private route: ActivatedRoute, private apiService: ApiService) {
    var id = this.route.snapshot.paramMap.get('id'); // get commit ID via URL
    this.apiService.getCommit(parseInt(id!)).subscribe((data: Commit) => {
      this.commit = data;
    });
  }

  ngOnInit() {}

  getTimestamp(dateTime: any): Date {
    return new Date(dateTime);
  }
}
