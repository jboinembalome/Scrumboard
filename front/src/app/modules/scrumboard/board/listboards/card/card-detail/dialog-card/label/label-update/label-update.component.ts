import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { debounceTime, tap } from 'rxjs/operators';
import { BlouppyUtils } from 'src/app/shared/utils/blouppyUtils';
import { ColourDto, LabelDto } from 'src/app/swagger';

@Component({
  selector: 'label-update',
  templateUrl: './label-update.component.html'
})
export class LabelUpdateComponent implements OnInit {

  @Input() label: LabelDto;
  @Output() labelUpdated = new EventEmitter<LabelDto>();
  @Output() labelDeleted = new EventEmitter<void>();

  labelForm: FormGroup;

  colors: ColourDto[] =
  [
      { colour: 'bg-blue-500' },
      { colour: 'bg-yellow-500' },
      { colour: 'bg-red-500' },
      { colour: 'bg-indigo-500' },
      { colour: 'bg-rose-500' },
      { colour: 'bg-pink-500' },
      { colour: 'bg-purple-500' },
      { colour: 'bg-violet-500' },
      { colour: 'bg-orange-500' },
      { colour: 'bg-amber-500' },
      { colour: 'bg-green-500' },
      { colour: 'bg-teal-500' },
      { colour: 'bg-gray-500' },
  ];
  
  constructor(private _formBuilder: FormBuilder) {
  }

  ngOnInit(): void {
    // Prepare the label form
    this.labelForm = this._formBuilder.group({
      name: ['', Validators.required],
    });

    // Fill the form
    this.labelForm.setValue({
      name: this.label.name,     
    });

    // Update label when there is a value change on the label form
    this.labelForm.valueChanges
    .subscribe((value) => {
        if (!BlouppyUtils.isNullOrEmpty(value.name)) {
          this.label.name = value.name;
          this.labelUpdated.emit(this.label);  
        }
      });
  }

  updateLabelColor(colour: ColourDto): void {
    this.label.colour = colour;
    this.labelUpdated.emit(this.label);
  }
}
