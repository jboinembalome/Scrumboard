import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import * as signalR from '@microsoft/signalr';
import { UpdateBoardCommandResponse } from 'src/app/swagger/model/updateBoardCommandResponse';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  connection: signalR.HubConnection;
  hubUpdatedBoard: BehaviorSubject<UpdateBoardCommandResponse>;

  constructor() {
    this.hubUpdatedBoard = new BehaviorSubject<UpdateBoardCommandResponse>(null);
  }

  // Establish a connection to the SignalR server hub
  public initiateSignalrConnection(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl("/boardHub") // TODO: use environment variable
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

  private setSignalrClientMethods(): void {
    this.connection.on('UpdatedBoard', (updateBoard: UpdateBoardCommandResponse) => {
      this.hubUpdatedBoard.next(updateBoard);
    });
  }
}