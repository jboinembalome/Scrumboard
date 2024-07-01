import { ChangeDetectionStrategy, Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { Observable } from 'rxjs';
import { DateTime } from 'luxon';
import { ActivityDto } from 'app/swagger';
import { MatIcon } from '@angular/material/icon';
import { NgClass, DatePipe } from '@angular/common';


@Component({
    selector: 'activities',
    templateUrl: './activities.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
    standalone: true,
    imports: [NgClass, MatIcon, DatePipe]
})
export class ActivitiesComponent {
  @Input() activities: ActivityDto[];

  /**
     * Returns whether the given dates are different days
     *
     * @param current
     * @param compare
     */
  isSameDay(current: string, compare: string): boolean {
    return DateTime.fromISO(current).hasSame(DateTime.fromISO(compare), 'day');
  }

  /**
   * Get the relative format of the given date
   *
   * @param date
   */
  getRelativeFormat(date: string): string {
    return DateTime.fromISO(date).toRelativeCalendar();
  }

  /**
  * Get the description of the activity
  *
  * @param activity
  */
  getDescription(activity: ActivityDto): string {
    const activityType = activity.activityType == 'NotFinished' ? 'not finished' : activity.activityType.toLowerCase();
    let description = '<strong>' + activity.adherent.firstName + ' ' + activity.adherent.lastName + '</strong> has ' + activityType + ' the ' + '<strong>' + activity.activityField.field + '</strong> ';
    switch (activity.activityType) {
      case 'Added':
        return description + '"' + activity.newValue + '"' + '.';
      case 'Removed':
        return description + '"' + activity.oldValue + '"' + '.';
      case 'Updated':
        return description + '"' + activity.oldValue + '"' + ' to ' + '"' + activity.newValue + '"' + '.';
      case 'Finished':
        return description + '"' + activity.newValue + '"' + '.';
      case 'NotFinished':
        return description + '"' + activity.newValue + '"' + '.';
      case 'Checked':
        return description + '"' + activity.newValue + '"' + '.';
      case 'Unchecked':
        return description + '"' + activity.newValue + '"' + '.';
    }
  }

  getIcon(activity: ActivityDto): string {
    switch (activity.activityType) {
      case 'Added':
        return 'add';
      case 'Removed':
        return 'delete';
      case 'Updated':
        return 'update';      
      case 'Finished':
        return 'checklist_rtl';
      case 'NotFinished':
        return 'rule';
      case 'Checked':
        return 'check_box';
      case 'Unchecked':
        return 'check_box_outline_blank';
    }
  }

  /**
   * Track by function for ngFor loops
   *
   * @param index
   * @param item
   */
  trackByFn(index: number, item: any): any {
    return item.id || index;
  }
}
