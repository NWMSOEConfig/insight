import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';

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

  constructor() {}

  onPublishClicked() {
    this.canPublish = false;
  }

  onEditClicked() {
    this.canEdit = true;
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
    this.canEdit = false;
    console.log('Save TODO');
  }

  GetSettings(): void {
    //this.apiService?.getQueue;
  }

  ngOnInit(): void {
    this.GetSettings();
  }
}
