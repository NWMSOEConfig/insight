import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-history-card',
  templateUrl: './history-card.component.html',
  styleUrls: ['./history-card.component.css'],
})
export class HistoryCardComponent {
  few = 4; // what "Few" means in getFirstFewFromBatch method
  @Input() commit: any;
  
  getTimestamp(dateTime: any): Date {
    return new Date(dateTime);
  }
  /**
   * Get the first few (defined above) entries from a list of settings
   * @param batch
   * @returns first few entries
   */
  getFirstFewFromBatch(batch: any): any {
    return batch.slice(0, this.few);
  }

  getRemainingBatchCount(batch: any): any {
    return batch.length - this.getFirstFewFromBatch(batch).length;
  }
}
