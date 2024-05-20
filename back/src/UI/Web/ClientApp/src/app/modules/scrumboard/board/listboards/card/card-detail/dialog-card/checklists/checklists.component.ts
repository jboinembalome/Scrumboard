import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ChecklistDto } from 'src/app/swagger';

@Component({
  selector: 'checklists',
  templateUrl: './checklists.component.html'
})
export class ChecklistsComponent {
  @Input() checklists: ChecklistDto[];
  @Output() checklistsUpdated = new EventEmitter<ChecklistDto[]>();

  constructor() {
  }

  removeChecklist(checklist: ChecklistDto): void {
    this.checklists.splice(this.checklists.indexOf(checklist), 1);

    this.checklistsUpdated.emit(this.checklists);
  }

  updateChecklist(checklist: ChecklistDto): void {
    let index = this.checklists.findIndex(c => c.name === checklist.name);
    if (index < 0)
      this.checklists[index] = checklist;

    this.checklistsUpdated.emit(this.checklists);
  }
}
