import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ColorPickerComponent } from './color-picker/color-picker.component';
import { SimpleColorPickerComponent } from './color-picker/simple-color-picker/simple-color-picker.component';
import { ToggleComponent } from './toggle/toggle.component';
import { SimpleCardComponent } from './cards/simple-card/simple-card.component';
import { AdherentSelectorComponent } from './adherent-selector/adherent-selector.component';

@NgModule({
    imports: [
    RouterModule,
    AdherentSelectorComponent,
    ColorPickerComponent,
    SimpleColorPickerComponent,
    ToggleComponent,
    SimpleCardComponent
],
    exports: [
        AdherentSelectorComponent,
        ColorPickerComponent,
        SimpleColorPickerComponent,
        ToggleComponent,
        SimpleCardComponent
    ]
})
export class ComponentModule
{
}
