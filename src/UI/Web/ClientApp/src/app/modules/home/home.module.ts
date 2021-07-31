import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ComponentModule  } from 'src/app/shared/components/component.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { HomeComponent } from './home.component';
import { homeRoutes } from './home.routing';

@NgModule({
    declarations: [
        HomeComponent
    ],
    imports     : [
        RouterModule.forChild(homeRoutes),
        ComponentModule,
        SharedModule
    ]
})
export class HomeModule
{
}
