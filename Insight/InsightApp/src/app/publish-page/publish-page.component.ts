import { Component, OnInit } from '@angular/core';
import { QueueEntry } from '../../models/queueEntry';
import { ApiService } from '../api-service/api.service';
import { PublishForm } from './publish-page.form';
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

  settingQueue: QueueEntry[] | null = null;
  publishForm: PublishForm;

  constructor(private apiService: ApiService) {
    this.publishForm = new PublishForm();
  }

  onPublishClicked() {
    this.showProgressBar = true;
    //this.apiService.postQueue(this.tenant.site ?? "", this.tenant.environment ?? "");
  }

  onEditClicked() {
    console.log('Edit TODO');
  }

  onDeleteClicked() {
    console.log('Undo TODO');
  }

  onUndoAllClicked() {
    //create confirmation modal
    console.log('Undo All TODO');
  }

  onSaveClicked() {
    console.log('Save TODO');
  }

  GetSettingQueue(): void {
    this.apiService.getQueue(this.tenant.site ?? "", this.tenant.environment ?? "").subscribe((x) => {
      this.settingQueue = x;
    });
  }

  ngOnInit(): void {
    this.GetSettingQueue();
  }
}
