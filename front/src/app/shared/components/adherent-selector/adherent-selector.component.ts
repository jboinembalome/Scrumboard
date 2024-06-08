import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AdherentDto } from 'app/swagger';
import { InitialPipe } from '../../pipes/initial.pipe';
import { MatCheckbox } from '@angular/material/checkbox';

@Component({
    selector: 'adherent-selector',
    templateUrl: './adherent-selector.component.html',
    standalone: true,
    imports: [MatCheckbox, InitialPipe]
})
export class AdherentSelectorComponent {

  @Input() selectedAdherents: AdherentDto[];
  @Input() adherents: AdherentDto[];
  @Input() unshift: boolean = true;
  @Output() adherentUpdated = new EventEmitter<AdherentDto[]>();

  urlAvatar: string = location.origin + "/api/adherents/avatar/";

  constructor() {
  }

  hasAdherent(member: AdherentDto): boolean {
    return !!this.selectedAdherents.find(m => m.id === member.id);
  }

  toggleMember(member: AdherentDto, checked: boolean): void {
    if (checked)
      this.addAdherent(member);
    else
      this.removeAdherent(member);

    this.adherentUpdated.emit(this.selectedAdherents);
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

  private addAdherent(adherent: AdherentDto): void {
    if(this.unshift)
      this.selectedAdherents.unshift(adherent);
    else
      this.selectedAdherents.push(adherent);
  }

  private removeAdherent(adherent: AdherentDto): void {
    const index = this.selectedAdherents.findIndex(m => m.id === adherent.id);

    if (index >= 0)
      this.selectedAdherents.splice(index, 1);
  }
}
