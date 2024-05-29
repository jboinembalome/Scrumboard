import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SearchbarComponent } from './searchbar.component';
import { MaterialModule } from 'app/shared/material/material.module';
import { SharedModule } from 'app/shared/shared.module';


@NgModule({
    declarations: [
        SearchbarComponent
    ],
    imports     : [
        RouterModule,
        SharedModule,
        MaterialModule
    ],
    exports     : [
        SearchbarComponent,
    ]
})
export class SearchbarModule
{
}
