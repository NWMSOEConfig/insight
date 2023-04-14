import { Component, OnInit, Input } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ApiService } from 'src/app/api-service/api.service';

export type FormInterface = { field1: string; field2: string };

@Component({
  selector: 'app-publish-page-form',
  templateUrl: './publish-page.form.html',
  styleUrls: ['./publish-page.form.css'],
})
export class PublishPageFormComponent implements OnInit {
  @Input() queueRender!: any;
  showProgressBar = false;
  disableProgressBar = false;
  referencedId!: string;
  commitDescription!: string;

  constructor(private apiService: ApiService, public snackBar: MatSnackBar) {}

  ngOnInit(): void {}

  openSnackbar(message: string) {
    this.snackBar.open(message, 'Dismiss', {
      duration: 5000,
    });
  }

  onPublishClicked() {
    this.showProgressBar = true;
    this.disableProgressBar = true;

    if (!this.referencedId) {
      this.openSnackbar('Reference ID must be a number');
      this.showProgressBar = false;
    } else if (parseInt(this.referencedId) <= 0) {
      this.openSnackbar('Reference ID must be a number greater than 0');
      this.showProgressBar = false;
    } else if (!this.commitDescription) {
      this.openSnackbar('Description is a required field');
      this.showProgressBar = false;
    } else {
      this.apiService
        .postPublish(this.commitDescription, parseInt(this.referencedId))
        .subscribe((data) => {
          this.openSnackbar('Changes have been successfully published');
          this.queueRender.queue = {};
          this.disableProgressBar = false;
          this.showProgressBar = false;
        });
    }
  }
}
