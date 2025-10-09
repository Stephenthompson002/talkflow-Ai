import { Component, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-chat',
  template: `
    <div>
      <button (click)='connect()' [disabled]='connected'>Connect</button>
      <input [(ngModel)]='message' placeholder='Type message' />
      <button (click)='send()'>Send</button>
      <ul>
        <li *ngFor='let m of messages'>{{m.sender}}: {{m.content}}</li>
      </ul>
    </div>
  `
})
export class ChatComponent implements OnInit {
  connection!: signalR.HubConnection;
  connected = false;
  message = '';
  messages: any[] = [];

  ngOnInit(){}

  connect() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('/hubs/chat')
      .withAutomaticReconnect()
      .build();
    this.connection.on('ReceiveMessage', (msg:any) => this.messages.push(msg));
    this.connection.start().then(() => this.connected = true);
  }

  send() {
    if (!this.connected) return;
    // For demo, use a fixed conversationId
    const convId = '00000000-0000-0000-0000-000000000001';
    this.connection.invoke('SendMessage', convId, 'agent', this.message);
    this.message = '';
  }
}
