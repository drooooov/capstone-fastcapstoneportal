import { Component, OnInit } from '@angular/core';
import { NotificationCommunicationService } from '../services/notification-communication.service';
import { Notifications } from '../models/notifications.model';
import { NotificationsService } from '../services/notifications.service';
import { Entreaty } from '../models/entreaty.model';
import { Logic } from '../models/logic';

@Component({
  selector: 'app-notifications-tab',
  templateUrl: './notifications-tab.component.html',
  styleUrls: ['./notifications-tab.component.css']
})
export class NotificationsTabComponent implements OnInit {

  notifications: Entreaty[];
  display = false;
  selectedNotification: Entreaty;

  constructor(private notify: NotificationCommunicationService,
    private entreaty: NotificationsService) { }

  ngOnInit() {
    this.notify.changeStudentEntreatyList();
    this.notify.currentEntreatyList.subscribe(data => {
      console.log(data);

      this.notifications = data;
      if (typeof this.notifications !== 'undefined') {

        this.display = this.notifications.length > 0;
        console.log(this.notifications);
      }
    });
    this.notify.currentNotification.subscribe(data => this.selectedNotification = data);
  }

  changeNotification(input: Entreaty) {
    this.notify.changeNotification(input);
  }
}
