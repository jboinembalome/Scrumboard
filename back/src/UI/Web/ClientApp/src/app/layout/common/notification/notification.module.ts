import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NotificationComponent } from './notification.component';
import { MaterialModule } from 'src/app/shared/material/material.module';
import { SharedModule } from 'src/app/shared/shared.module';


@NgModule({
    declarations: [
        NotificationComponent
    ],
    imports     : [
        RouterModule,
        SharedModule,
        MaterialModule
    ],
    exports     : [
        NotificationComponent,
    ]
})
export class NotificationModule
{
}
