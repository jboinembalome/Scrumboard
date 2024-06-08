import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { ComponentModule  } from 'app/shared/components/component.module';
import { SharedModule } from 'app/shared/shared.module';
import { FetchDataComponent } from './fetch-data.component';
import { fetchDataRoutes } from './fetch-data.routing';

@NgModule({
    imports: [
        RouterModule.forChild(fetchDataRoutes),
        MatButtonModule,
        ComponentModule,
        SharedModule,
        FetchDataComponent
    ]
})
export class FetchDataModule
{
}
