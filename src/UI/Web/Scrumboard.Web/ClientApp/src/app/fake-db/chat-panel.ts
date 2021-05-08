export class ChatPanelFakeDb
{
    public static contacts = [
        {
            'id'    : '5725a680b3249760ea21de52',
            'name'  : 'Bébébé <3',
            'avatar': 'assets/images/avatars/bebe.jpg',
            'status': 'online',
            'mood'  : 'Lorem ipsum dolor sit amet, consectetur adipiscing elit...',
            'unread': '2'
        },
        {
            'id'    : '5725a680606588342058356d',
            'name'  : 'Guyliane',
            'avatar': 'assets/images/avatars/guyliane.jpg',
            'status': 'do-not-disturb',
            'mood'  : 'Lorem ipsum dolor sit amet, consectetur adipiscing elit...'
        },
        {
            'id'    : '5725a68009e20d0a9e9acf2a',
            'name'  : 'Erika',
            'avatar': 'assets/images/avatars/erika.jpg',
            'status': 'do-not-disturb',
            'mood'  : 'Lorem ipsum dolor sit amet, consectetur adipiscing elit...'
        },
        {
            'id'    : '5725a6809fdd915739187ed5',
            'name'  : 'Jeffrey',
            'avatar': 'assets/images/avatars/jeffrey_zed.jpg',
            'status': 'offline',
            'mood'  : 'Lorem ipsum dolor sit amet, consectetur adipiscing elit...',
            'unread': '3'
        },
        {
            'id'    : '5725a68007920cf75051da64',
            'name'  : 'Marco Che',
            'avatar': 'assets/images/avatars/marco_che.jpg',
            'status': 'offline',
            'mood'  : 'Lorem ipsum dolor sit amet, consectetur adipiscing elit...',
            'unread': '1'
        },
        {
            'id'    : '5725a68031fdbb1db2c1af47',
            'name'  : 'Vanessa Che',
            'avatar': 'assets/images/avatars/vanessa_che.jpg',
            'status': 'offline',
            'mood'  : 'Lorem ipsum dolor sit amet, consectetur adipiscing elit...'
        },
        {
            'id'    : '5725a680bc670af746c435e2',
            'name'  : 'Erwann',
            'avatar': 'assets/images/avatars/erwann.jpg',
            'status': 'online',
            'mood'  : 'Lorem ipsum dolor sit amet, consectetur adipiscing elit...'
        },
        {
            'id'    : '5725a680e7eb988a58ddf303',
            'name'  : 'Pablo',
            'avatar': 'assets/images/avatars/pablo.png',
            'status': 'away',
            'mood'  : 'Lorem ipsum dolor sit amet, consectetur adipiscing elit...'
        },
        {
            'id'    : '5725a680dcb077889f758961',
            'name'  : 'Corentin',
            'avatar': 'assets/images/avatars/corentin.jpg',
            'status': 'offline',
            'mood'  : 'Lorem ipsum dolor sit amet, consectetur adipiscing elit...'
        },
        {
            'id'    : '5725a6806acf030f9341e925',
            'name'  : 'Cédric',
            'avatar': 'assets/images/avatars/cedric.jpg',
            'status': 'away',
            'mood'  : 'Lorem ipsum dolor sit amet, consectetur adipiscing elit...'
        },
        {
            'id'    : '5725a680ae1ae9a3c960d487',
            'name'  : 'Erwan',
            'avatar': 'assets/images/avatars/erwan_bijot.jpg',
            'status': 'offline',
            'mood'  : 'Lorem ipsum dolor sit amet, consectetur adipiscing elit...'
        },
        {
            'id'    : '5725a680b8d240c011dd224b',
            'name'  : 'Patrice',
            'avatar': 'assets/images/avatars/patrice.jpg',
            'status': 'online',
            'mood'  : 'Lorem ipsum dolor sit amet, consectetur adipiscing elit...'
        },
        {
            'id'    : '5725a6809413bf8a0a5272b1',
            'name'  : 'Jimmy',
            'avatar': 'assets/images/avatars/jimmy_zed.jpg',
            'status': 'online',
            'mood'  : 'Lorem ipsum dolor sit amet, consectetur adipiscing elit...'
        }
    ];

    public static chats = [
        {
            'id'    : '1725a680b3249760ea21de52',
            'dialog': [
                {
                    'who'    : '5725a680b3249760ea21de52',
                    'message': 'Quickly come to the meeting room 1B, we have a big server issue',
                    'time'   : '2017-03-22T08:54:28.299Z'
                },
                {
                    'who'    : '5725a6802d10e277a0f35724',
                    'message': 'I’m having breakfast right now, can’t you wait for 10 minutes?',
                    'time'   : '2017-03-22T08:55:28.299Z'
                },
                {
                    'who'    : '5725a6802d10e277a0f35724',
                    'message': 'I’m having breakfast right now, can’t you wait for 10 minutes?',
                    'time'   : '2017-03-22T08:55:28.299Z'
                },
                {
                    'who'    : '5725a680b3249760ea21de52',
                    'message': 'We are losing money! Quick!',
                    'time'   : '2017-03-22T09:00:28.299Z'
                },
                {
                    'who'    : '5725a6802d10e277a0f35724',
                    'message': 'It’s not my money, you know. I will eat my breakfast and then I will come to the meeting room.',
                    'time'   : '2017-03-22T09:02:28.299Z'
                },
                {
                    'who'    : '5725a680b3249760ea21de52',
                    'message': 'You are the worst!',
                    'time'   : '2017-03-22T09:05:28.299Z'
                },
                {
                    'who'    : '5725a680b3249760ea21de52',
                    'message': 'We are losing money! Quick!',
                    'time'   : '2017-03-22T09:15:28.299Z'
                },
                {
                    'who'    : '5725a680b3249760ea21de52',
                    'message': 'You are the worst!',
                    'time'   : '2017-03-22T09:05:28.299Z'
                },
                {
                    'who'    : '5725a680b3249760ea21de52',
                    'message': 'We are losing money! Quick!',
                    'time'   : '2017-03-22T09:15:28.299Z'
                },
                {
                    'who'    : '5725a6802d10e277a0f35724',
                    'message': 'It’s not my money, you know. I will eat my breakfast and then I will come to the meeting room.',
                    'time'   : '2017-03-22T09:20:28.299Z'
                },
                {
                    'who'    : '5725a680b3249760ea21de52',
                    'message': 'You are the worst!',
                    'time'   : '2017-03-22T09:22:28.299Z'
                },
                {
                    'who'    : '5725a680b3249760ea21de52',
                    'message': 'We are losing money! Quick!',
                    'time'   : '2017-03-22T09:25:28.299Z'
                },
                {
                    'who'    : '5725a6802d10e277a0f35724',
                    'message': 'It’s not my money, you know. I will eat my breakfast and then I will come to the meeting room.',
                    'time'   : '2017-03-22T09:27:28.299Z'
                },
                {
                    'who'    : '5725a680b3249760ea21de52',
                    'message': 'You are the worst!',
                    'time'   : '2017-03-22T09:33:28.299Z'
                },
                {
                    'who'    : '5725a680b3249760ea21de52',
                    'message': 'You are the worst!',
                    'time'   : '2017-03-22T09:33:28.299Z'
                },
                {
                    'who'    : '5725a680b3249760ea21de52',
                    'message': 'We are losing money! Quick!',
                    'time'   : '2017-03-22T09:35:28.299Z'
                },
                {
                    'who'    : '5725a6802d10e277a0f35724',
                    'message': 'It’s not my money, you know. I will eat my breakfast and then I will come to the meeting room.',
                    'time'   : '2017-03-22T09:45:28.299Z'
                },
                {
                    'who'    : '5725a680b3249760ea21de52',
                    'message': 'You are the worst!',
                    'time'   : '2017-03-22T10:00:28.299Z'
                }
            ]
        },
        {
            'id'    : '2725a680b8d240c011dd2243',
            'dialog': [
                {
                    'who'    : '5725a680606588342058356d',
                    'message': 'Quickly come to the meeting room 1B, we have a big server issue',
                    'time'   : '2017-04-22T01:00:00.299Z'
                },
                {
                    'who'    : '5725a6802d10e277a0f35724',
                    'message': 'I’m having breakfast right now, can’t you wait for 10 minutes?',
                    'time'   : '2017-04-22T01:05:00.299Z'
                },
                {
                    'who'    : '5725a680606588342058356d',
                    'message': 'We are losing money! Quick!',
                    'time'   : '2017-04-22T01:10:00.299Z'
                }
            ]
        },
        {
            'id'    : '3725a6809413bf8a0a5272b4',
            'dialog': [
                {
                    'who'    : '5725a68009e20d0a9e9acf2a',
                    'message': 'Quickly come to the meeting room 1B, we have a big server issue',
                    'time'   : '2017-04-22T02:10:00.299Z'
                }
            ]
        }
    ];

    public static user = [
        {
            'id'      : '5725a6802d10e277a0f35724',
            'name'    : 'Jimmy Boinembalome',
            'avatar'  : 'assets/images/avatars/jimmy_zed.jpg',
            'status'  : 'online',
            'mood'    : '',
            'chatList': [
                {
                    'chatId'         : '1725a680b3249760ea21de52',
                    'contactId'      : '5725a680b3249760ea21de52',
                    'lastMessageTime': '2017-06-12T02:10:18.931Z'
                },
                {
                    'chatId'         : '2725a680b8d240c011dd2243',
                    'contactId'      : '5725a680606588342058356d',
                    'lastMessageTime': '2017-02-18T10:30:18.931Z'
                },
                {
                    'chatId'         : '3725a6809413bf8a0a5272b4',
                    'contactId'      : '5725a68009e20d0a9e9acf2a',
                    'lastMessageTime': '2017-03-18T12:30:18.931Z'
                }
            ]
        }
    ];

}
