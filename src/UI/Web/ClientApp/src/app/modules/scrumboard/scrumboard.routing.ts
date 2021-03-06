import { Route } from '@angular/router';
import { BoardComponent } from './board/board.component';
import { BoardResolver } from './board/board.resolvers';
import { CardDetailComponent } from './board/listboards/card/card-detail/card-detail.component';
import { BoardsComponent } from './boards/boards.component';

export const scrumboardRoutes: Route[] = [
    {
        path: '',
        component: BoardsComponent,
    },
    {
        path: ':boardId',
        component: BoardComponent,
        resolve: {
            board: BoardResolver
        },
        children: [
            {
                path: 'card/:cardId',
                component: CardDetailComponent,
            }
        ]
    },
];
