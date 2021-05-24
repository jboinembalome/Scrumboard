import { FuseNavigation } from '@fuse/types';

export const navigation: FuseNavigation[] = [
    {
        id: 'authentication',
        title: 'Authentication',
        translate: 'NAV.AUTHENTICATION',
        type: 'collapsable',
        icon: 'lock',
        children: [
            {
                id: 'login',
                title: 'Login',
                translate: 'NAV.LOGIN',
                type: 'item',
                url: '/authentication/login'
            },
            {
                id: 'register',
                title: 'Register',
                translate: 'NAV.REGISTER',
                type: 'item',
                url: '/authentication/register'
            },
            {
                id: 'logout',
                title: 'Logout',
                translate: 'NAV.LOGOUT',
                type: 'item',
                url: '/authentication/logout'
            }
        ]
    },
    {
        id       : 'applications',
        title    : 'Applications',
        translate: 'NAV.APPLICATIONS',
        type     : 'group',
        children: [
            {
                id: 'home',
                title: 'Home',
                translate: 'NAV.HOME',
                type: 'item',
                icon: 'home',
                url: '/home'
            },
            {
                id       : 'sample',
                title    : 'Sample',
                translate: 'NAV.SAMPLE.TITLE',
                type     : 'item',
                icon     : 'face',
                url      : '/sample',
                badge    : {
                    title    : '25',
                    translate: 'NAV.SAMPLE.BADGE',
                    bg       : '#F44336',
                    fg       : '#FFFFFF'
                }
            },
            {
                id: 'scrumboard',
                title: 'Scrumboard',
                translate: 'NAV.SCRUMBOARD',
                type: 'item',
                icon: 'assessment',
                url: '/apps/scrumboard'
            }
        ]
    },
    {
        id: 'documentation',
        title: 'Documentation',
        translate: 'NAV.DOCUMENTATION',
        type: 'group',
        children: [
            {
                id: 'swagger',
                title: 'Swagger UI',
                translate: 'NAV.SWAGGER_UI',
                type: 'item',
                icon: 'Description',
                url: '/swagger'
            }
        ]
    }
];