import { Navigation } from "app/core/navigation/models/navigation.model";

const navigationPaths: Navigation[] =
[
    { name: 'Home', url: '/home', icon: 'home' },
    { name: 'Counter', url: '/counter', icon: 'pin' },
    { name: 'Fetch data', url: '/fetch-data', icon: 'downloading' },
    { name: 'Memory game', url: '/memory-game', icon: 'videogame_asset' },
    { name: 'Scrumboard', url: '/scrumboard', icon: 'view_week' }
];

export const NavigationPaths: Navigation[] = navigationPaths;
