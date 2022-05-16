import { ChatService } from './../../services/chat.service';
import { SignalrService } from './../../services/signalr.service';
import { AuthService } from 'src/app/services/auth.service';
import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs';
import { Message } from 'src/app/interfaces/message.interface';
import { MessageResponse } from 'src/app/interfaces/message.response.interface';



@Component({
  selector: 'app-chatroom',
  templateUrl: './chatroom.component.html',
  styleUrls: ['./chatroom.component.scss']
})
export class ChatroomComponent implements OnInit {

  connId!: string;
  inputMessage: string = '';
  messages: MessageResponse[] = [];


  constructor(private authSvc: AuthService,
    private http: HttpClient,
    private chatSvc: ChatService,
    private signalrSvc: SignalrService) { }

  ngOnInit(): void {
    this.signalrSvc.startConnection();
    this.signalrSvc.connId$.subscribe(connId =>{
      this.connId = connId;
    });
    this.chatSvc.messages$.subscribe( messages => {
      this.messages = messages;
    });
    this.chatSvc.getMessages();
  }

  sendMessage() {
    if(this.isBlank(this.inputMessage))
      return;

    return this.http.post<Message>("http://localhost:22593/api/chatroom/sendmessage", {message: this.inputMessage})
      .subscribe( data => {
        console.log(data)
      });
  }

  private isBlank(input: string): boolean {
    return (!input || /^\s*$/.test(input));
  }
}
