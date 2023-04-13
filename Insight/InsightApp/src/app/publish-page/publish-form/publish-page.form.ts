import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ApiService } from 'src/app/api-service/api.service';

export type FormInterface = { field1: string; field2: string };

@Component({
  selector: 'app-publish-page-form',
  templateUrl: './publish-page.form.html',
  styleUrls: ['./publish-page.form.css'],
})
export class PublishPageFormComponent implements OnInit {
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

    if (!this.referencedId) {
      this.openSnackbar('Reference ID must be a number');
      this.showProgressBar = false;
      this.disableProgressBar = true;
    } else if (parseInt(this.referencedId) <= 0) {
      this.openSnackbar('Reference ID must be a number greater than 0');
      this.showProgressBar = false;
      this.disableProgressBar = true;
    } else if (!this.commitDescription) {
      this.openSnackbar('Description is a required field');
      this.showProgressBar = false;
      this.disableProgressBar = true;
    } else {
      this.apiService
        .postPublish(this.commitDescription, parseInt(this.referencedId))
        .subscribe((data) => {
          this.showProgressBar = false;
          this.openSnackbar('Changes have been successfully published');
          window.location.reload();
        });
    }
  }
}
