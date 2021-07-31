import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { ComponentModule  } from 'src/app/shared/components/component.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { FetchDataComponent } from './fetch-data.component';
import { fetchDataRoutes } from './fetch-data.routing';

@NgModule({
    declarations: [
        FetchDataComponent
    ],
    imports     : [
        RouterModule.forChild(fetchDataRoutes),
        MatButtonModule,
        ComponentModule,
        SharedModule
    ]
})
export class FetchDataModule
{
}
