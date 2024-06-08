import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { ComponentModule  } from 'app/shared/components/component.module';

import { FetchDataComponent } from './fetch-data.component';
import { fetchDataRoutes } from './fetch-data.routing';

@NgModule({
    imports: [
    RouterModule.forChild(fetchDataRoutes),
    MatButtonModule,
    ComponentModule,
    FetchDataComponent
]
})
export class FetchDataModule
{
}
