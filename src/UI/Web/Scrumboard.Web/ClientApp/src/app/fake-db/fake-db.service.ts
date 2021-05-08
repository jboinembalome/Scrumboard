import { InMemoryDbService } from 'angular-in-memory-web-api';

import { ScrumboardFakeDb } from 'app/fake-db/scrumboard';
import { QuickPanelFakeDb } from 'app/fake-db/quick-panel';
import { ChatPanelFakeDb } from 'app/fake-db/chat-panel';



export class FakeDbService implements InMemoryDbService
{
    createDb(): any
    {
        return {
            // Scrumboard
            'scrumboard-boards': ScrumboardFakeDb.boards,

            // Quick Panel
            'quick-panel-notes': QuickPanelFakeDb.notes,
            'quick-panel-events': QuickPanelFakeDb.events,

            // Chat Panel
            'chat-panel-contacts': ChatPanelFakeDb.contacts,
            'chat-panel-chats': ChatPanelFakeDb.chats,
            'chat-panel-user': ChatPanelFakeDb.user
        };
    }
}
