import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { mockCommits } from '../history-page/mock-commits';

@Component({
  selector: 'app-commit-page',
  templateUrl: './commit-page.component.html',
  styleUrls: ['./commit-page.component.css'],
})
export class CommitPageComponent implements OnInit {
  commit: any;

  constructor(private route: ActivatedRoute) {
    var id = this.route.snapshot.paramMap.get('id'); // get commit ID via URL
    this.commit = mockCommits.filter((c) => c.id == id).at(0); // get commit from mockCommits via ID
  }
  ngOnInit() {}

  getTimestamp(dateTime: any): Date {
    return new Date(dateTime);
  }
}
