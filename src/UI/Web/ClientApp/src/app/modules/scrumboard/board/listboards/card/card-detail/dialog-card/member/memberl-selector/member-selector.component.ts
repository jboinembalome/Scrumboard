import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AdherentDto } from 'src/app/swagger';

@Component({
  selector: 'member-selector',
  templateUrl: './member-selector.component.html'
})
export class MemberSelectorComponent {

  @Input() selectedMembers: AdherentDto[];
  @Input() members: AdherentDto[];
  @Output() memberUpdated = new EventEmitter<any[]>();

  urlAvatar: string = location.origin + "/api/adherents/avatar/";

  constructor() {
  }

  hasMember(member: AdherentDto): boolean {
    return !!this.selectedMembers.find(m => m.id === member.id);
  }

  toggleMember(member: AdherentDto, checked: boolean): void {
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
  trackByFn(index: number, item: AdherentDto): any {
    return item.id || index;
  }

  private addMemberToCard(member: AdherentDto): void {
    this.selectedMembers.unshift(member);
  }

  private removeMemberFromCard(member: AdherentDto): void {
    const index = this.selectedMembers.findIndex(m => m.id === member.id);

    if (index >= 0)
      this.selectedMembers.splice(index, 1);
  }
}
