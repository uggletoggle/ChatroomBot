import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { MessageResponse } from '../interfaces/message.response.interface';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  private messages!: BehaviorSubject<Array<MessageResponse>>;
  public messages$!: Observable<Array<MessageResponse>>;
  private MAX_LENGTH = 50;

  constructor(
    private http : HttpClient
  ) {
    this.messages = new BehaviorSubject<MessageResponse[]>([]);
    this.messages$ = this.messages.asObservable();
  }

  public addMessage(message: MessageResponse) {
    let messages = this.messages.value;

    if (messages.length >= 50){
      messages.pop();
    }

    console.log(message.text)

    messages.unshift(message);

    this.messages.next(messages);
  }

  public getMessages() {
    return this.http.get<MessageResponse[]>("http://localhost:22593/api/chatroom/")
      .subscribe( data => {
        let messages = this.messages.value.concat(data);
        this.messages.next(messages);
      });
  }
}
