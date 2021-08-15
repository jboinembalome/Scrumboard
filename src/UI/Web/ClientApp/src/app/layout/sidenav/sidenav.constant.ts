import { Navigation } from './models/navigation.model';

const navigationPaths: Navigation[] =
[
    { Name: 'Home', Href: '/home', Icon: 'home' },
    { Name: 'Counter', Href: '/counter', Icon: 'pin' },
    { Name: 'Fetch data', Href: '/fetch-data', Icon: 'downloading' },
    { Name: 'Scrumboard', Href: '/scrumboard', Icon: 'view_week' }
];

export const NavigationPaths: Navigation[] = navigationPaths;
