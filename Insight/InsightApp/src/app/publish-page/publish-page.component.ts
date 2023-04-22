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
  //organize settings changed by their category?
  errorMessage: string | undefined;
  commitMessage: string | undefined;
  canPublish = true;
  canEdit = false;

  settings: any[] = [
    {
      name: 'setting one',
      newvalue: 'value one',
      oldvalue: 'value old one',
    },
    {
      name: 'setting two',
      newvalue: 'value two',
      oldvalue: 'value old two',
    },
    {
      name: 'setting three',
      newvalue: 'value three',
      oldvalue: 'value old three',
    },
  ];
  selected: any;
  showProgressBar = false;
  tenant = getTenant();
  queue!: QueueEntry;

  constructor(private apiService: ApiService) {}

  onDeleteClicked(settingName: String) {
    // this.apiService.
    console.log('Undo TODO');
  }

  onUndoAllClicked() {
    //create confirmation modal
    console.log('Undo All TODO');
  }

  onSaveClicked() {
    this.canEdit = false;
    console.log('Save TODO');
  }

  GetSettings(): void {
    //this.apiService?.getQueue;
  }

  ngOnInit(): void {
    this.apiService.getQueue().subscribe((data) => {
      this.queue = data;
    });
  }
}
