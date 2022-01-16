import { NgModule } from '@angular/core';
import { DragDropModule } from '@angular/cdk/drag-drop';
import {ScrollingModule} from '@angular/cdk/scrolling';
import { RouterModule } from '@angular/router';
import { ComponentModule } from 'src/app/shared/components/component.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { MaterialModule } from 'src/app/shared/material/material.module';

import { scrumboardRoutes } from './scrumboard.routing';

import { ScrumboardComponent } from './scrumboard.component';
import { BoardsComponent } from './boards/boards.component';
import { BoardComponent } from './board/board.component';
import { CardComponent } from './board/listboards/card/card.component';
import { CardAddComponent } from './board/listboards/card/card-add/card-add.component';
import { DialogCardComponent } from './board/listboards/card/card-detail/dialog-card/dialog-card.component';
import { CardDetailComponent } from './board/listboards/card/card-detail/card-detail.component';
import { ListBoardsComponent } from './board/listboards/listboards.component';
import { ListBoardAddComponent } from './board/listboards/listboard-add/listboard-add.component';

import { BoardsFilterPipe } from './boards/boards.pipe';
import { StringColorPipe } from 'src/app/shared/pipes/string-color.pipe';
import { SettingComponent } from './board/setting/setting.component';
import { ChecklistAddComponent } from './board/listboards/card/card-detail/dialog-card/checklists/checklist/checklist-add/checklist-add.component';
import { LabelSelectorComponent } from './board/listboards/card/card-detail/dialog-card/label/label-selector/label-selector.component';
import { LabelAddComponent } from './board/listboards/card/card-detail/dialog-card/label/label-add/label-add.component';
import { ChecklistComponent } from './board/listboards/card/card-detail/dialog-card/checklists/checklist/checklist.component';
import { ChecklistsComponent } from './board/listboards/card/card-detail/dialog-card/checklists/checklists.component';
import { CommentsComponent } from './board/listboards/card/card-detail/dialog-card/comments/comments.component';
import { CommentComponent } from './board/listboards/card/card-detail/dialog-card/comments/comment/comment.component';
import { CommentAddComponent } from './board/listboards/card/card-detail/dialog-card/comments/comment/comment-add/comment-add.component';

@NgModule({
    declarations: [
        ScrumboardComponent,
        BoardComponent,
        BoardsComponent,
        LabelAddComponent,
        LabelSelectorComponent,
        ListBoardsComponent,
        CardComponent,
        CardAddComponent,
        ChecklistComponent,
        ChecklistsComponent,
        ChecklistAddComponent,
        CommentComponent,
        CommentAddComponent,
        CommentsComponent,
        DialogCardComponent,
        CardDetailComponent,
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
