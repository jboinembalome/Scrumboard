import { Routes } from '@angular/router';
import { BoardComponent } from './board/board.component';
import { BoardResolver } from './board/board.resolvers';
import { CardDetailComponent } from './board/listboards/card/card-detail/card-detail.component';
import { BoardsComponent } from './boards/boards.component';
import { CardDetailResolver } from './board/listboards/card/card-detail/card-detail.resolvers';

export default [
    {
        path: '',
        component: BoardsComponent,
    },
    {
        path: ':boardId',
        component: BoardComponent,
        resolve: {
            board: BoardResolver
        }
    },
    {
        path: ':boardId/card/:cardId',
        component: CardDetailComponent,
        resolve: {
            card: CardDetailResolver
        }
    }
] as Routes;
