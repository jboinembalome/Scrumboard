import { Route } from '@angular/router';

import { AuthGuard } from 'src/app/core/auth/guards/auth.guard';
import { LayoutComponent } from './layout/layout.component';

export const appRoutes: Route[] = [
     // Redirect empty path to '/home'
     {path: '', pathMatch : 'full', redirectTo: 'home'},

    // Auth routes for guests
    {
        path: '',
        component: LayoutComponent,
        children: [
            {path: 'home', loadChildren: () => import('./modules/home/home.module').then(m => m.HomeModule)},
            {path: 'counter', loadChildren: () => import('./modules/counter/counter.module').then(m => m.CounterModule)},
            {path: 'fetch-data', loadChildren: () => import('./modules/fetch-data/fetch-data.module').then(m => m.FetchDataModule)},
            {path: 'memory-game', loadChildren: () => import('./modules/memory-game/memory-game.module').then(m => m.MemoryGameModule)},
            {path: 'auth', loadChildren: () => import('./core/auth/auth.module').then(m => m.AuthModule)},
        ]
    },

    // Auth routes for authenticated users
        // Admin routes
        {
            path       : '',
            //canActivate: [AuthGuard], // Do not forget to uncomment this
            //canActivateChild: [AuthGuard], // Do not forget to uncomment this
            component  : LayoutComponent,
            children   : [
                {path: 'scrumboard', loadChildren: () => import('./modules/scrumboard/scrumboard.module').then(m => m.ScrumboardModule)},
            ]
        }
];
