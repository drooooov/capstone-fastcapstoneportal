import { Injectable, EventEmitter } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Notifications } from '../models/notifications.model';
import { NotificationCommunicationService } from './notification-communication.service';
import { StudentService } from './student.service';
import { GroupService } from './group.service';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalRServiceService {

  apiUrl = environment.API_URL;
  // messageReceived = new EventEmitter<Notifications>();
  // connectionEstablished = new EventEmitter<Boolean>();

  // private connectionIsEstablished = false;
  // private _hubConnection: HubConnection;


  // // sendChatMessage(message: Notifications) {
  // //   this._hubConnection.invoke('SendMessage', message);
  // // }

  // private createConnection() {
  //   this._hubConnection = new HubConnectionBuilder()
  //     .withUrl('http://18.223.170.23:5000/SignalServer')
  //     .build();
  // }

  // private startConnection(): void {
  //   this._hubConnection
  //     .start()
  //     .then(() => {
  //       this.connectionIsEstablished = true;
  //       console.log('Hub connection started');
  //       this.connectionEstablished.emit(true);
  //     })
  //     .catch(err => {
  //       console.log('Error while establishing connection, retrying...');
  //       console.log(err);
  //       // setTimeout(this.startConnection(), 5000);
  //     });
  // }

  // private registerOnServerEvents(): void {
  //   this._hubConnection.on('ReceiveMessage', (data: any) => {
  //     this.messageReceived.emit(data);
  //   });
  // }

  constructor(private notify: NotificationCommunicationService,
    private groupService: GroupService,
    private studentService: StudentService) {
  }
  private connection: signalR.HubConnection;
  messageReceived = new EventEmitter<Notification>();


  requestNotification(message: Notification) {
    this.connection.invoke('Request', message);
  }

  inviteNotification(message: Notification) {
    this.connection.invoke('Invite', message);
  }

  connect() {
    if (!this.connection) {
      const token = '' + localStorage.getItem('token');
      // console.log(token);
      this.connection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost/SignalServer', {accessTokenFactory: () => token})
      .build();

      this.connection.on('ReceiveNotification', (data) => {
        console.log('Received');
        this.notify.changeNotificationList();
        this.notify.changeStudentEntreatyList();
        this.notify.changeReadStatus();
        this.studentService
        .getStudent(localStorage.getItem('id'))
        .subscribe(studentData => {
          this.studentService.changeStudent(studentData);
          this.groupService.getGroup(studentData.groupId).subscribe(groupData => {
            this.groupService.changeGroup(groupData);
            console.log(groupData);
          });
        });
      });

      this.connection.start().catch(err => console.error(err));
    }
  }

  private registerOnServerEvents(): void {
    this.connection.on('ReceiveNotification', (data: any) => {
      console.log(data);
      this.messageReceived.emit(data);
    });
  }


  disconnect() {
    if (this.connection) {
      this.connection.stop();
      this.connection = null;
    }
  }
}
