import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ChecklistDto } from 'app/swagger';
import { MatIcon } from '@angular/material/icon';
import { MatInput } from '@angular/material/input';
import { MatFormField, MatError } from '@angular/material/form-field';


@Component({
    selector: 'checklist-add',
    templateUrl: './checklist-add.component.html',
    standalone: true,
    imports: [FormsModule, ReactiveFormsModule, MatFormField, MatInput, MatError, MatIcon]
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
