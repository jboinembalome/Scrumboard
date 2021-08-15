import { Route } from '@angular/router';
import { BoardComponent } from './board/board.component';
import { BoardsComponent } from './boards/boards.component';

export const scrumboardRoutes: Route[] = [
    {
        path     : '',
        component: BoardsComponent
    },
    {
        path     : ':id',
        component: BoardComponent,
        children : [
            // Add Card Children
        ]
    },
];
