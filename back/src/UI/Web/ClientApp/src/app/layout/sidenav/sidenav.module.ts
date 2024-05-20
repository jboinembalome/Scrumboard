import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SidenavComponent } from './sidenav.component';
import { ToolbarModule } from '../toolbar/toolbar.module';
import { MaterialModule } from 'src/app/shared/material/material.module';
import { SharedModule } from 'src/app/shared/shared.module';


@NgModule({
    declarations: [
        SidenavComponent
    ],
    imports     : [
        RouterModule,
        ToolbarModule,
        SharedModule,
        MaterialModule
    ],
    exports     : [
        SidenavComponent,
    ]
})
export class SidenavModule
{
}
