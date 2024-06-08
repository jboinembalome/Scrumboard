import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, tap } from 'rxjs/operators';
import { BlouppyUtils } from 'app/shared/utils/blouppyUtils';
import { ColourDto, LabelDto } from 'app/swagger';
import { MatInput } from '@angular/material/input';
import { MatFormField, MatError } from '@angular/material/form-field';
import { MatDivider } from '@angular/material/divider';
import { MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatMenuTrigger, MatMenu } from '@angular/material/menu';

@Component({
    selector: 'label-update',
    templateUrl: './label-update.component.html',
    standalone: true,
    imports: [MatMenuTrigger, MatIcon, MatMenu, MatIconButton, MatDivider, FormsModule, ReactiveFormsModule, MatFormField, MatInput, MatError]
})
export class LabelUpdateComponent implements OnInit {

  @Input() label: LabelDto;
  @Output() labelUpdated = new EventEmitter<LabelDto>();
  @Output() labelDeleted = new EventEmitter<void>();

  labelForm: UntypedFormGroup;

  colors: ColourDto[] =
  [
      { colour: 'bg-blue-800/30' },
      { colour: 'bg-yellow-800/30' },
      { colour: 'bg-red-800/30' },
      { colour: 'bg-indigo-800/30' },
      { colour: 'bg-rose-800/30' },
      { colour: 'bg-pink-800/30' },
      { colour: 'bg-purple-800/30' },
      { colour: 'bg-violet-800/30' },
      { colour: 'bg-orange-800/30' },
      { colour: 'bg-amber-800/30' },
      { colour: 'bg-green-800/30' },
      { colour: 'bg-teal-800/30' },
      { colour: 'bg-gray-800/30' },
  ];
  
  constructor(private _formBuilder: UntypedFormBuilder) {
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
