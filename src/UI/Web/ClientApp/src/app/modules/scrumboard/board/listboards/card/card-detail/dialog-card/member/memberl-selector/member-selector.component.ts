import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'member-selector',
  templateUrl: './member-selector.component.html'
})
export class MemberSelectorComponent {

  @Input() selectedMembers: any[];
  @Input() members: any[];
  @Output() memberUpdated = new EventEmitter<any[]>();

  constructor() {
  }

  hasMember(member: any): boolean {
    return !!this.selectedMembers.find(m => m.name === member.name);
  }

  toggleMember(member: any, checked: boolean): void {
    if (checked)
      this.addMemberToCard(member);
    else
      this.removeMemberFromCard(member);

    this.memberUpdated.emit(this.selectedMembers);
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

  private addMemberToCard(member: any): void {
    this.selectedMembers.unshift(member);
  }

  private removeMemberFromCard(member: any): void {
    const index = this.selectedMembers.findIndex(m => m === member);

    if (index >= 0)
      this.selectedMembers.splice(index, 1);
  }
}
