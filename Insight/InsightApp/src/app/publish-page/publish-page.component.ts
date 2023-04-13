import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api-service/api.service';
import { getTenant } from '../tenant-singleton';

@Component({
  selector: 'app-publish-page',
  templateUrl: './publish-page.component.html',
  styleUrls: ['./publish-page.component.css'],
})
export class PublishPageComponent implements OnInit {
  selected: any;
  showProgressBar = false;
  tenant = getTenant();
  queue!: any;

  constructor(private apiService: ApiService) {}

  onDeleteClicked() {
    console.log('Delete');
  }

  ngOnInit(): void {
    this.apiService.getQueue().subscribe((data) => {
      this.queue = data;
      console.log(this.queue);
    });
  }
}
