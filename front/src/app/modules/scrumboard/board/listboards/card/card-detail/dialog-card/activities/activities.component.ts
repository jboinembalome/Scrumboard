import { ChangeDetectionStrategy, Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { Observable } from 'rxjs';
import * as moment from 'moment';
import { ActivityDto } from 'src/app/swagger';


@Component({
  selector: 'activities',
  templateUrl: './activities.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
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
    return moment(current, moment.ISO_8601).isSame(moment(compare, moment.ISO_8601), 'day');
  }

  /**
   * Get the relative format of the given date
   *
   * @param date
   */
  getRelativeFormat(date: string): string {
    const today = moment().startOf('day');
    const yesterday = moment().subtract(1, 'day').startOf('day');

    // Is today?
    if (moment(date, moment.ISO_8601).isSame(today, 'day')) {
      return 'Today';
    }

    // Is yesterday?
    if (moment(date, moment.ISO_8601).isSame(yesterday, 'day')) {
      return 'Yesterday';
    }

    return moment(date, moment.ISO_8601).fromNow();
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
