import { Navigation } from './models/navigation.model';

let navigationPaths: Navigation[] =
[
    { Name: 'Home', Href: '/home', Icon: 'home' },
    { Name: 'Counter', Href: '/counter', Icon: 'pin' },
    { Name: 'Fetch data', Href: '/fetch-data', Icon: 'downloading' },
];

export const NavigationPaths: Navigation[] = navigationPaths;
