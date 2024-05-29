import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ToolbarComponent } from './toolbar.component';
import { SearchbarModule } from '../common/searchbar/searchbar.module';
import { NotificationModule } from '../common/notification/notification.module';
import { UserProfileModule } from '../common/user-profile/user-profile.module';
import { MaterialModule } from 'app/shared/material/material.module';
import { SharedModule } from 'app/shared/shared.module';


@NgModule({
    declarations: [
        ToolbarComponent
    ],
    imports     : [
        RouterModule,
        SearchbarModule,
        NotificationModule,
        UserProfileModule,
        SharedModule,
        MaterialModule
    ],
    exports     : [
        ToolbarComponent,
    ]
})
export class ToolbarModule
{
}
