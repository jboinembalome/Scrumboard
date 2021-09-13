import { Route } from '@angular/router';
import { BoardComponent } from './board/board.component';
import { CardDetailComponent } from './board/listboards/card/card-detail/card-detail.component';
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
            {
                path     : 'card/:cardId',
                component: CardDetailComponent,
                //resolve  : {
                //    card: ScrumboardCardResolver
                //}
            }
        ]
    },
];
