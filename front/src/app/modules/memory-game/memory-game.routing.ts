import { Route } from '@angular/router';
import { MemoryGameComponent } from './memory-game.component';

export const counterRoutes: Route[] = [
    {
        path     : '',
        component: MemoryGameComponent
    }
];
