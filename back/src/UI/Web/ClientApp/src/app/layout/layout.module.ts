import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LayoutComponent } from './layout.component';
import { SidenavModule  } from './sidenav/sidenav.module';

@NgModule({
    declarations: [
        LayoutComponent,
    ],
    imports     : [
        RouterModule,
        SidenavModule,
    ],
    exports     : [
        LayoutComponent,
    ]
})
export class LayoutModule
{
}
