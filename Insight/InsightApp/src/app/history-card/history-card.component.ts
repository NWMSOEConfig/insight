import { Component, Input, OnInit } from '@angular/core';
import { Commit } from '../../models/commit';
import { ApiService } from '../api-service/api.service';

@Component({
  selector: 'app-history-card',
  templateUrl: './history-card.component.html',
  styleUrls: ['./history-card.component.css'],
})
export class HistoryCardComponent {
  @Input() commit: any;
  @Input() numberOfSettingsToDisplay: any = 4; // Have 4 settings deplayed by default

  getTimestamp(dateTime: any): Date {
    return new Date(dateTime);
  }
  /**
   * Get the first few (defined above) entries from a list of settings
   * @param batch
   * @returns first few entries
   */
  getFirstFewFromBatch(batch: any): any {
    if (batch.length > this.numberOfSettingsToDisplay) {
      return batch.slice(0, this.numberOfSettingsToDisplay);
    } else {
      return batch;
    }
  }

  getRemainingBatchCount(batch: any): any {
    return batch.length - this.getFirstFewFromBatch(batch).length;
  }

  
}
