import { ChatService } from './chat.service';
import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr"
import { Observable, Subject } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class SignalrService {
    private hubConnection!: signalR.HubConnection;

    private connId!: Subject<string>;
    public connId$!: Observable<string>;


    constructor(private chatSvc: ChatService) {
      this.connId = new Subject();
      this.connId$ = this.connId.asObservable();
    }

    public startConnection = () => {
      this.hubConnection = new signalR.HubConnectionBuilder()
                              .withUrl('http://localhost:22593/chatroom/guid')
                              .build();

      this.hubConnection.on('ReceiveConnID', (connId) => {
        this.connId.next(connId);
      });

      this.hubConnection.on('ReceiveMessage', (message) => {
        console.log(message);
        this.chatSvc.addMessage(message);
      });

      this.hubConnection
        .start()
        .then(() => console.log('Connection started'))
        .catch(err => console.log('Error while starting connection: ' + err))
    }

}
