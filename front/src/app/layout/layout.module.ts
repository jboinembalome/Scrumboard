import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LayoutComponent } from './layout.component';
import { SidenavModule  } from './sidenav/sidenav.module';
import { EmptyLayoutComponent } from './empty/empty.component';

@NgModule({
    declarations: [
        LayoutComponent,
        EmptyLayoutComponent
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
