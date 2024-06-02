import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SidenavComponent } from './sidenav.component';
import { ToolbarModule } from '../toolbar/toolbar.module';
import { MaterialModule } from 'app/shared/material/material.module';
import { SharedModule } from 'app/shared/shared.module';
import { BlouppyIconComponent } from "../common/blouppy-icon/blouppy-icon.component";


@NgModule({
    declarations: [
        SidenavComponent
    ],
    exports: [
        SidenavComponent,
    ],
    imports: [
        RouterModule,
        ToolbarModule,
        SharedModule,
        MaterialModule,
        BlouppyIconComponent
    ]
})
export class SidenavModule
{
}
