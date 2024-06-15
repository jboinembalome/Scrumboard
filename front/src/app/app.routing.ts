import { Route } from '@angular/router';

import { AuthGuard } from 'app/core/auth/guards/auth.guard';
import { VerticalLayoutComponent } from './layout/vertical-layout/vertical-layout.component';
import { EmptyLayoutComponent } from './layout/empty-layout/empty-layout.component';

export const appRoutes: Route[] = [
     // Redirect empty path to '/login'
     {path: '', pathMatch : 'full', redirectTo: 'login'},

    // Auth routes for guests
    {
        path: '',
        component: EmptyLayoutComponent,
        children: [
            {path: 'auth', loadChildren: () => import('./core/auth/auth.routes')},
        ]
    },

    // Auth routes for authenticated users
        // Admin routes
        {
            path       : '',
            canActivate: [AuthGuard], // Do not forget to uncomment this
            canActivateChild: [AuthGuard], // Do not forget to uncomment this
            component  : VerticalLayoutComponent,
            children   : [
                {path: 'home', loadChildren: () => import('./modules/home/home.routes')},
                {path: 'counter', loadChildren: () => import('./modules/counter/counter.routes')},
                {path: 'fetch-data', loadChildren: () => import('./modules/fetch-data/fetch-data.routes')},
                {path: 'memory-game', loadChildren: () => import('./modules/memory-game/memory-game.routes')},
                {path: 'scrumboard', loadChildren: () => import('./modules/scrumboard/scrumboard.routes')},
            ]
        }
];
