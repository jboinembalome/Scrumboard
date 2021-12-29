import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ColorPickerComponent } from './color-picker/color-picker.component';
import { SimpleColorPickerComponent } from './color-picker/simple-color-picker/simple-color-picker.component';
import { TitleComponent } from './title/title.component';
import { ToggleComponent } from './toggle/toggle.component';
import { SelectComponent } from './selects/select/select.component';
import { SelectCheckboxComponent } from './selects/select-checkbox/select-checkbox.component';
import { SimpleCardComponent } from './cards/simple-card/simple-card.component';
import { InputWithIconComponent } from './inputs/input-with-icon/input-with-icon.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { MaterialModule } from 'src/app/shared/material/material.module';

@NgModule({
    declarations: [
        ColorPickerComponent,
        SimpleColorPickerComponent,
        TitleComponent,
        ToggleComponent,
        SelectComponent,
        SelectCheckboxComponent,
        InputWithIconComponent,
        SimpleCardComponent
    ],
    imports: [
        RouterModule,
        SharedModule,
        MaterialModule
    ],
    exports: [
        ColorPickerComponent,
        SimpleColorPickerComponent,
        TitleComponent,
        ToggleComponent,
        SelectComponent,
        SelectCheckboxComponent,
        InputWithIconComponent,
        SimpleCardComponent
    ]
})
export class ComponentModule
{
}
