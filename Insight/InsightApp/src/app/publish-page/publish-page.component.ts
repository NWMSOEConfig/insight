import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api-service/api.service';
import { getTenant } from '../tenant-singleton';
import { QueueEntry } from 'src/models/queueEntry';

@Component({
  selector: 'app-publish-page',
  templateUrl: './publish-page.component.html',
  styleUrls: ['./publish-page.component.css'],
})
export class PublishPageComponent implements OnInit {
  selected: any;
  showProgressBar = false;
  tenant = getTenant();
  queue!: QueueEntry;

  constructor(private apiService: ApiService) { }

  onDeleteClicked(settingName: string) {
    this.apiService.deleteSettingFromQueue(settingName).subscribe(() => {
      this.delay(500);
      this.ngOnInit();
    });
  }

  delay(ms: number)
  {
    return new Promise ( resolve => setTimeout(resolve, ms));
  }

  ngOnInit(): void {
    this.apiService.getQueue().subscribe((data) => {
      this.queue = data;
    });
  }
}
