import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { ChecklistDto } from 'app/swagger';


@Component({
  selector: 'checklist-add',
  templateUrl: './checklist-add.component.html'
})
export class ChecklistAddComponent implements OnInit {
  checklistForm: UntypedFormGroup;
  @Output() checklistAdded = new EventEmitter<ChecklistDto>();
  @Output() checklistCanceled = new EventEmitter<void>();

  constructor(
    private _formBuilder: UntypedFormBuilder) {
  }

  ngOnInit(): void {
    // Prepare the checklist form
    this.checklistForm = this._formBuilder.group({
      name: ['', Validators.required],
    });

    // Fill the form
    this.checklistForm.setValue({
      name: '',     
    });
  }

  addChecklist(): void {

    const checklist: ChecklistDto = {
      name: this.checklistForm.get('name').value,
      checklistItems: []
    };

    this.checklistAdded.emit(checklist);
    this.checklistForm.reset();
  }
}
