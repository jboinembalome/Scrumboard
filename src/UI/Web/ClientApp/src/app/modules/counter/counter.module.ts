import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { ComponentModule  } from 'src/app/shared/components/component.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { CounterComponent } from './counter.component';
import { counterRoutes } from './counter.routing';

@NgModule({
    declarations: [
        CounterComponent
    ],
    imports     : [
        RouterModule.forChild(counterRoutes),
        MatButtonModule,
        ComponentModule,
        SharedModule
    ]
})
export class CounterModule
{
}
