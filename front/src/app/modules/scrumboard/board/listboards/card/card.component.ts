import { Component, Input } from '@angular/core';
import { DateTime } from 'luxon';
import { CardDto } from 'app/swagger';
import { InitialPipe } from '../../../../../shared/pipes/initial.pipe';
import { MatIcon } from '@angular/material/icon';
import { MatTooltip } from '@angular/material/tooltip';
import { NgClass, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CdkDrag, CdkDragPlaceholder } from '@angular/cdk/drag-drop';

@Component({
    selector: 'scrumboard-card',
    templateUrl: './card.component.html',
    styleUrls: ['./card.component.scss'],
    standalone: true,
    imports: [
      CdkDrag, 
      RouterLink, 
      CdkDragPlaceholder, 
      NgClass, 
      MatTooltip, 
      MatIcon, 
      DatePipe, 
      InitialPipe]
})
export class CardComponent {
  @Input() card: CardDto;

  urlAvatar: string = location.origin + "/api/users/avatar/";
  
  constructor() {
  }

  /**
  * Check if the given ISO_8601 date string is overdue
  *
  * @param date
  */
  isOverdue(date: Date): boolean {
    return DateTime.fromISO(date).startOf('day') < DateTime.now().startOf('day');
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
