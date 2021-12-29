import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LabelDto } from 'src/app/swagger';


@Component({
  selector: 'label-add',
  templateUrl: './label-add.component.html'
})
export class LabelAddComponent implements OnInit {
  labelForm: FormGroup;
  @Output() labelAdded = new EventEmitter<LabelDto>();
  @Output() labelCanceled = new EventEmitter<void>();

  constructor(
    private _formBuilder: FormBuilder) {
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
