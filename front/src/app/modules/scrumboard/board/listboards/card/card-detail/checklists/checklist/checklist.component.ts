import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ChecklistDto, ChecklistItemDto } from 'app/swagger';
import { MatCheckbox } from '@angular/material/checkbox';
import { MatInput } from '@angular/material/input';
import { MatFormField, MatPrefix, MatSuffix } from '@angular/material/form-field';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatMenuTrigger, MatMenu, MatMenuItem } from '@angular/material/menu';
import { MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';

@Component({
    selector: 'checklist',
    templateUrl: './checklist.component.html',
    styleUrls: ['./checklist.component.scss'],
    standalone: true,
    imports: [MatIcon, MatIconButton, MatMenuTrigger, MatMenu, MatMenuItem, MatProgressBar, MatFormField, MatInput, MatCheckbox, MatPrefix, MatSuffix]
})
export class ChecklistComponent implements OnInit {
  @Input() checklist: ChecklistDto;
  @Input() checklists: ChecklistDto[];
  @Output() checklistUpdated = new EventEmitter<ChecklistDto>();
  @Output() checklistRemoved = new EventEmitter<ChecklistDto>();

  checklistItems: number;
  checklistItemsChecked: number;

  constructor() {
  }

  ngOnInit(): void {
    this.updateCheckedCount(this.checklist);
  }

  updateCheckedCount(checkList: ChecklistDto): void {
    this.checklistItems = checkList.checklistItems.length;
    this.checklistItemsChecked = checkList.checklistItems.filter((c) => c.isChecked).length;
  }

  removeChecklistItem(checkItem: ChecklistItemDto, checklist: ChecklistDto): void {
    checklist.checklistItems.splice(checklist.checklistItems.indexOf(checkItem), 1);

    this.updateCheckedCount(checklist);
    this.checklistUpdated.emit(this.checklist);
  }

  addCheckItem(newItem: string, checkList: ChecklistDto): void {
    if (!newItem || newItem === '')
      return;

    const newCheckItem: ChecklistItemDto= {
      name: newItem,
      isChecked: false
    };

    checkList.checklistItems.push(newCheckItem);

    this.updateCheckedCount(checkList);
    this.checklistUpdated.emit(this.checklist);
  }

  updateCheckItem(updateItem: string, checkItem: ChecklistItemDto, checklist: ChecklistDto): void {
    if (!updateItem || updateItem === '')
      return;

    const checklistItems = checklist.checklistItems;
    const updatecheckItem = checklistItems.find(x => x === checkItem);

    if (updatecheckItem !== undefined)
      updatecheckItem.name = updateItem;

    this.checklistUpdated.emit(this.checklist);
  }

  toggleCheckItem(checkList: ChecklistDto, updateCheckItem: ChecklistItemDto, checked: boolean): void {
    const checklistItems = checkList.checklistItems;
    const checkItem = checklistItems.find(x => x.id === updateCheckItem.id);

    if (checklistItems !== undefined) {
      checkItem.isChecked = checked;
      this.updateCheckedCount(checkList);
    }

    this.checklistUpdated.emit(this.checklist);
  }
}
