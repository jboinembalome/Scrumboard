import { NgModule } from '@angular/core';
import { DragDropModule } from '@angular/cdk/drag-drop';
import {ScrollingModule} from '@angular/cdk/scrolling';
import { RouterModule } from '@angular/router';
import { ComponentModule } from 'src/app/shared/components/component.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { MaterialModule } from 'src/app/shared/material/material.module';

import { ScrumboardComponent } from './scrumboard.component';
import { BoardsComponent } from './boards/boards.component';
import { BoardComponent } from './board/board.component';
import { ListBoardsComponent } from './board/listboards/listboards.component';
import { CardComponent } from './board/listboards/card/card.component';
import { CardAddComponent } from './board/listboards/card/card-add/card-add.component';
import { ListBoardAddComponent } from './board/listboards/listboard-add/listboard-add.component';

import { BoardsFilterPipe } from './boards/boards.pipe';
import { scrumboardRoutes } from './scrumboard.routing';
import { StringColorPipe } from 'src/app/shared/pipes/string-color.pipe';
import { SettingComponent } from './board/setting/setting.component';

@NgModule({
    declarations: [
        ScrumboardComponent,
        BoardComponent,
        BoardsComponent,
        ListBoardsComponent,
        CardComponent,
        CardAddComponent,
        ListBoardAddComponent,
        SettingComponent,

        BoardsFilterPipe
    ],
    imports: [
        RouterModule.forChild(scrumboardRoutes),
        DragDropModule,
        ScrollingModule,
        ComponentModule,
        SharedModule,
        MaterialModule
    ],
    providers: [
        StringColorPipe
    ]
})
export class ScrumboardModule {
}
