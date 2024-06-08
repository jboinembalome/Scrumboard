import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SidenavComponent } from './sidenav.component';
import { MaterialModule } from 'app/shared/material/material.module';
import { SharedModule } from 'app/shared/shared.module';
import { BlouppyIconComponent } from "../common/blouppy-icon/blouppy-icon.component";
import { UserProfileModule } from '../common/user-profile/user-profile.module';


@NgModule({
    exports: [
        SidenavComponent,
    ],
    imports: [
        RouterModule,
        UserProfileModule,
        SharedModule,
        MaterialModule,
        BlouppyIconComponent,
        SidenavComponent
    ]
})
export class SidenavModule
{
}
