import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ColorPickerComponent } from './color-picker/color-picker.component';
import { SimpleColorPickerComponent } from './color-picker/simple-color-picker/simple-color-picker.component';
import { ToggleComponent } from './toggle/toggle.component';
import { SimpleCardComponent } from './cards/simple-card/simple-card.component';
import { UserSelectorComponent } from './user-selector/user-selector.component';

@NgModule({
    imports: [
    RouterModule,
    UserSelectorComponent,
    ColorPickerComponent,
    SimpleColorPickerComponent,
    ToggleComponent,
    SimpleCardComponent
],
    exports: [
        UserSelectorComponent,
        ColorPickerComponent,
        SimpleColorPickerComponent,
        ToggleComponent,
        SimpleCardComponent
    ]
})
export class ComponentModule
{
}
