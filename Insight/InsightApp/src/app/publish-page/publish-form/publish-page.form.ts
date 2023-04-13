import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-publish-page-form',
  templateUrl: './publish-page.form.html',
  styleUrls: ['./publish-page.form.css'],
})
export class PublishPageFormComponent implements OnInit {
  showProgressBar = false;

  ngOnInit(): void {}

  onPublishClicked() {
    this.showProgressBar = true;
    // this.apiService.postQueue(this.tenant.site ?? "", this.tenant.environment ?? "");
  }
}
