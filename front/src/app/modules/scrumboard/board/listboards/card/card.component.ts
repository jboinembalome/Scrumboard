import { Component, Input } from '@angular/core';
import * as moment from 'moment';
import { CardDto } from 'app/swagger';

@Component({
  selector: 'scrumboard-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.scss']
})
export class CardComponent {
  @Input() card: CardDto;

  urlAvatar: string = location.origin + "/api/adherents/avatar/";
  
  constructor() {
  }

  /**
  * Check if the given ISO_8601 date string is overdue
  *
  * @param date
  */
  isOverdue(date: string): boolean {
    return moment(date, moment.ISO_8601).isBefore(moment(), 'days');
  }

  /**
  * Tracks by function for ngFor loops.
  *
  * @param index
  * @param item
  */
  trackByFn(index: number, item: any): any {
    return item.id || index;
  }
}
