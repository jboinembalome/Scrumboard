import { Component, EventEmitter, Input, Output } from '@angular/core';
import { UserDto } from 'app/swagger';
import { InitialPipe } from '../../pipes/initial.pipe';
import { MatCheckbox } from '@angular/material/checkbox';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
    selector: 'user-selector',
    templateUrl: './user-selector.component.html',
    standalone: true,
    imports: [MatCheckbox, MatTooltipModule, InitialPipe]
})
export class UserSelectorComponent {

  @Input() selectedUsers: UserDto[];
  @Input() users: UserDto[];
  @Input() unshift: boolean = true;
  @Output() userUpdated = new EventEmitter<UserDto[]>();

  urlAvatar: string = location.origin + "/api/adherents/avatar/";

  constructor() {
  }

  hasUser(user: UserDto): boolean {
    return !!this.selectedUsers.find(x => x.id === user.id);
  }

  toggleUser(user: UserDto, checked: boolean): void {
    if (checked)
      this.addUser(user);
    else
      this.removeUser(user);

    this.userUpdated.emit(this.selectedUsers);
  }

  /**
  * Tracks by function for ngFor loops.
  *
  * @param index
  * @param item
  */
  trackByFn(index: number, item: UserDto): any {
    return item.id || index;
  }

  private addUser(user: UserDto): void {
    if(this.unshift)
      this.selectedUsers.unshift(user);
    else
      this.selectedUsers.push(user);
  }

  private removeUser(user: UserDto): void {
    const index = this.selectedUsers.findIndex(x => x.id === user.id);

    if (index >= 0)
      this.selectedUsers.splice(index, 1);
  }
}
