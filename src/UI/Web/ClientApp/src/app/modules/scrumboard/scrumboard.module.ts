import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ComponentModule  } from 'src/app/shared/components/component.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { MaterialModule } from 'src/app/shared/material/material.module';
import { ScrumboardComponent } from './scrumboard.component';
import { BoardsComponent } from './boards/boards.component';
import { BoardComponent } from './board/board.component';
import { BoardsFilterPipe } from './boards/boards.pipe';
import { scrumboardRoutes } from './scrumboard.routing';
import { StringColorPipe } from 'src/app/shared/pipes/string-color.pipe';

@NgModule({
    declarations: [
        ScrumboardComponent,
        BoardComponent,
        BoardsComponent,

        BoardsFilterPipe
    ],
    imports     : [
        RouterModule.forChild(scrumboardRoutes),
        
        ComponentModule,
        SharedModule,
        MaterialModule
    ],
    providers: [
        StringColorPipe
      ]
})
export class ScrumboardModule
{
}
