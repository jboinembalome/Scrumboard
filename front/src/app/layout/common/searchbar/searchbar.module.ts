import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SearchbarComponent } from './searchbar.component';


@NgModule({
    declarations: [
        SearchbarComponent
    ],
    imports     : [
        RouterModule
    ],
    exports     : [
        SearchbarComponent,
    ]
})
export class SearchbarModule
{
}
