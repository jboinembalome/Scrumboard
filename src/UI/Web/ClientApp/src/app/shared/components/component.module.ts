import { NgModule } from '@angular/core';
import { TitleComponent } from './title/title.component';
import { ToggleComponent } from './toggle/toggle.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
    declarations: [
        TitleComponent,
        ToggleComponent
    ],
    imports: [
        SharedModule
    ],
    exports: [
        TitleComponent,
        ToggleComponent
    ]
})
export class ComponentModule
{
}
