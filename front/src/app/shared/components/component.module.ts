import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ColorPickerComponent } from './color-picker/color-picker.component';
import { SimpleColorPickerComponent } from './color-picker/simple-color-picker/simple-color-picker.component';
import { ToggleComponent } from './toggle/toggle.component';
import { SelectComponent } from './selects/select/select.component';
import { SimpleCardComponent } from './cards/simple-card/simple-card.component';
import { AdherentSelectorComponent } from './adherent-selector/adherent-selector.component';

@NgModule({
    imports: [
    RouterModule,
    AdherentSelectorComponent,
    ColorPickerComponent,
    SimpleColorPickerComponent,
    ToggleComponent,
    SelectComponent,
    SimpleCardComponent
],
    exports: [
        AdherentSelectorComponent,
        ColorPickerComponent,
        SimpleColorPickerComponent,
        ToggleComponent,
        SelectComponent,
        SimpleCardComponent
    ]
})
export class ComponentModule
{
}
