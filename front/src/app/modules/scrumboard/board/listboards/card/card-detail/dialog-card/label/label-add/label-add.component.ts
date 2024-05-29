import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { LabelDto } from 'app/swagger';


@Component({
  selector: 'label-add',
  templateUrl: './label-add.component.html'
})
export class LabelAddComponent implements OnInit {
  labelForm: UntypedFormGroup;
  @Output() labelAdded = new EventEmitter<LabelDto>();
  @Output() labelCanceled = new EventEmitter<void>();

  constructor(
    private _formBuilder: UntypedFormBuilder) {
  }

  ngOnInit(): void {
    // Prepare the label form
    this.labelForm = this._formBuilder.group({
      name: ['', Validators.required],
    });

    // Fill the form
    this.labelForm.setValue({
      name: '',     
    });
  }

   addLabel(): void {
    const label: LabelDto = {
      name: this.labelForm.get('name').value, 
      colour: { colour: 'bg-gray-500' } 
    };

    this.labelAdded.emit(label);
    this.labelForm.reset();
  }
}
