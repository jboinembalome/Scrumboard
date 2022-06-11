import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  connection: signalR.HubConnection;
  hubChatMessage: BehaviorSubject<string>;

  constructor() {
    this.hubChatMessage = new BehaviorSubject<string>(null);
  }

  // Establish a connection to the SignalR server hub
  public initiateSignalrConnection(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub") // TODO: use environment variable
        .build();

      this.setSignalrClientMethods();

      this.connection
        .start()
        .then(() => {
          console.log(
            `SignalR connection success! connectionId: ${this.connection.connectionId} `
          );
          resolve();
        })
        .catch((error) => {
          console.log(`SignalR connection error: ${error}`);
          reject();
        });
    });
  }

  // This method will implement the methods defined in the ISignalrDemoHub interface in the SignalrDemo.Server .NET solution
  private setSignalrClientMethods(): void {
    this.connection.on('ReceiveMessage', (user: string, message: string) => {
      this.hubChatMessage.next(`${user} says ${message}`);
    });
  }
}