import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ComponentModule  } from 'app/shared/components/component.module';

import { HomeComponent } from './home.component';
import { homeRoutes } from './home.routing';

@NgModule({
    imports: [
    RouterModule.forChild(homeRoutes),
    ComponentModule,
    HomeComponent
]
})
export class HomeModule
{
}
