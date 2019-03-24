import { Component, OnInit } from '@angular/core';
import { NotificationCommunicationService } from '../services/notification-communication.service';
import { GroupService } from '../services/group.service';
import { ComponentCommunicationService } from '../services/component-communication.service';
import { Router } from '@angular/router';
import { NotificationsService } from '../services/notifications.service';
import { Notifications } from '../models/notifications.model';
import { NotificationType } from '../enums/NotificationType.enum';
import { NotificationColor } from '../enums/NotificationColor.enum';

@Component({
  selector: 'app-notifications-list',
  templateUrl: './notifications-list.component.html',
  styleUrls: ['./notifications-list.component.css']
})
export class NotificationsListComponent implements OnInit {

  notificationsList: Notifications[];
  unreadNotifications = [];

  public NOTIFY_TYPE = NotificationType;
  public NOTIFY_COLOR = NotificationColor;

  constructor(private notify: NotificationCommunicationService,
    private groupService: GroupService,
    private commsService: ComponentCommunicationService,
    private _router: Router,
    private entrapy: NotificationsService) { }

  ngOnInit() {
    this.notify.changeNotificationList();
    this.notify.currentNotificationList
      .subscribe(arg => this.notificationsList = arg);
    this.entrapy.getUnreadNotifications().subscribe(data => {
      this.unreadNotifications = data.map(x => {
        return x.id;
      });
      console.log(this.unreadNotifications);

      this.readNotifications();
    });
  }

  readNotifications() {
    this.entrapy.readNotifications(this.unreadNotifications).subscribe(data => {
      console.log(data);
      this.notify.changeReadStatus();
    });
  }

  isRead(givenNotification: Notifications) {
    const value = this.unreadNotifications.includes(givenNotification.id);
    console.log(value);
    return value;
  }

}
