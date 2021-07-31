import { Route } from '@angular/router';

import { AuthGuard } from 'src/app/core/auth/guards/auth.guard';
import { LayoutComponent } from './layout/layout.component';

export const appRoutes: Route[] = [
     // Redirect empty path to '/home'
     {path: '', pathMatch : 'full', redirectTo: 'home'},

    // Auth routes for guests
    {
        path: '',
        //canActivate: [NoAuthGuard],
        //canActivateChild: [NoAuthGuard],
        component: LayoutComponent,
        children: [
            {path: 'home', loadChildren: () => import('./modules/home/home.module').then(m => m.HomeModule)},
            {path: 'counter', loadChildren: () => import('./modules/counter/counter.module').then(m => m.CounterModule)},
            {path: 'auth', loadChildren: () => import('./core/auth/auth.module').then(m => m.AuthModule)},
            //{path: 'forgot-password', loadChildren: () => import('app/modules/auth/forgot-password/forgot-password.module').then(m => m.AuthForgotPasswordModule)},
            //{path: 'reset-password', loadChildren: () => import('app/modules/auth/reset-password/reset-password.module').then(m => m.AuthResetPasswordModule)},
            //{path: 'sign-in', loadChildren: () => import('app/modules/auth/sign-in/sign-in.module').then(m => m.AuthSignInModule)},
            //{path: 'sign-up', loadChildren: () => import('app/modules/auth/sign-up/sign-up.module').then(m => m.AuthSignUpModule)}
        ]
    },

    // Auth routes for authenticated users
        // Admin routes
        {
            path       : '',
            //canActivate: [AuthGuard],
            //canActivateChild: [AuthGuard],
            component  : LayoutComponent,
            children   : [
                {path: 'fetch-data', loadChildren: () => import('./modules/fetch-data/fetch-data.module').then(m => m.FetchDataModule)},
                // Dashboards
                /*
                {path: 'dashboards', children: [
                    {path: 'project', loadChildren: () => import('app/modules/admin/dashboards/project/project.module').then(m => m.ProjectModule)},
                    {path: 'analytics', loadChildren: () => import('app/modules/admin/dashboards/analytics/analytics.module').then(m => m.AnalyticsModule)},
                ]},
                */
                // Apps
                /*
                {path: 'apps', children: [
                    //{path: 'calendar', loadChildren: () => import('app/modules/admin/apps/calendar/calendar.module').then(m => m.CalendarModule)},
                    //{path: 'chat', loadChildren: () => import('app/modules/admin/apps/chat/chat.module').then(m => m.ChatModule)},
                    //{path: 'contacts', loadChildren: () => import('app/modules/admin/apps/contacts/contacts.module').then(m => m.ContactsModule)},
                    //{path: 'file-manager', loadChildren: () => import('app/modules/admin/apps/file-manager/file-manager.module').then(m => m.FileManagerModule)},
                    //{path: 'help-center', loadChildren: () => import('app/modules/admin/apps/help-center/help-center.module').then(m => m.HelpCenterModule)},
                    //{path: 'notes', loadChildren: () => import('app/modules/admin/apps/notes/notes.module').then(m => m.NotesModule)},
                    //{path: 'scrumboard', loadChildren: () => import('app/modules/admin/apps/scrumboard/scrumboard.module').then(m => m.ScrumboardModule)},
                    //{path: 'tasks', loadChildren: () => import('app/modules/admin/apps/tasks/tasks.module').then(m => m.TasksModule)},
                ]},
                */
                // Pages
                /*
                {path: 'pages', children: [
    
                    // Activities
                    //{path: 'activities', loadChildren: () => import('app/modules/admin/pages/activities/activities.module').then(m => m.ActivitiesModule)},
        
                    // Coming Soon
                    //{path: 'coming-soon', loadChildren: () => import('app/modules/admin/pages/coming-soon/coming-soon.module').then(m => m.ComingSoonModule)},
        
                    // Profile
                    //{path: 'profile', loadChildren: () => import('app/modules/admin/pages/profile/profile.module').then(m => m.ProfileModule)},
    
                    // Settings
                    //{path: 'settings', loadChildren: () => import('app/modules/admin/pages/settings/settings.module').then(m => m.SettingsModule)},
                ]},
                */
                // Documentation
                /*
                {path: 'docs', children: [
    
                    // Changelog
                    {path: 'changelog', loadChildren: () => import('app/modules/admin/docs/changelog/changelog.module').then(m => m.ChangelogModule)},
    
                    // Guides
                    {path: 'guides', loadChildren: () => import('app/modules/admin/docs/guides/guides.module').then(m => m.GuidesModule)}
                ]},
                */
                // 404 & Catch all
                //{path: '404-not-found', pathMatch: 'full', loadChildren: () => import('app/modules/admin/pages/error/error-404/error-404.module').then(m => m.Error404Module)},
                //{path: '**', redirectTo: '404-not-found'}
            ]
        }
];
