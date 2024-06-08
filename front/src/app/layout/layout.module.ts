import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LayoutComponent } from './layout.component';
import { SidenavModule  } from './sidenav/sidenav.module';
import { EmptyLayoutComponent } from './empty/empty.component';

@NgModule({
    imports: [
        RouterModule,
        SidenavModule,
        LayoutComponent,
        EmptyLayoutComponent,
    ],
    exports: [
        LayoutComponent,
    ]
})
export class LayoutModule
{
}
