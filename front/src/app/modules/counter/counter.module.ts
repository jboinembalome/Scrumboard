import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { ComponentModule  } from 'app/shared/components/component.module';
import { SharedModule } from 'app/shared/shared.module';
import { CounterComponent } from './counter.component';
import { counterRoutes } from './counter.routing';

@NgModule({
    imports: [
        RouterModule.forChild(counterRoutes),
        MatButtonModule,
        ComponentModule,
        SharedModule,
        CounterComponent
    ]
})
export class CounterModule
{
}
