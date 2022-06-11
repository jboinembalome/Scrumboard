import { Component, OnInit } from '@angular/core';
import { SignalrService } from './signalr.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})

export class ChatComponent implements OnInit {

  sendMessageDisabled: boolean = true;
  hubHelloMessage: string;
  user: string = '';
  message: string = '';
  messages: string[] = [];

  constructor( private signalrService: SignalrService) {
  }

  ngOnInit(): void {
    this.signalrService.initiateSignalrConnection()
      .then(() => this.sendMessageDisabled = false)
      .catch(() => this.sendMessageDisabled = true);

    this.signalrService.hubChatMessage
      .subscribe((message: string) => this.messages.push(message));
  }

  onSendMessageClick(): void {
    this.sendMessageDisabled = true;

    this.signalrService.connection
      .invoke('SendMessage', this.user, this.message)
      .then(() => {
        this.sendMessageDisabled = false;
      })
      .catch((error) => {
        console.log(`SignalrDemoHub.SendMessage() error: ${error}`);
      });
  }
}
