import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NotificationComponent } from './notification.component';


@NgModule({
    declarations: [
        NotificationComponent
    ],
    imports     : [
        RouterModule
    ],
    exports     : [
        NotificationComponent,
    ]
})
export class NotificationModule
{
}
