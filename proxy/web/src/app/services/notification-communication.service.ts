import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Notifications } from '../models/notifications.model';
import { Entreaty } from '../models/entreaty.model';
import { NotificationsService } from './notifications.service';
import { Logic } from '../models/logic';

@Injectable({
  providedIn: 'root'
})
export class NotificationCommunicationService {

  defaultNotification: Entreaty;
  private notificationSource = new BehaviorSubject(this.defaultNotification);
  currentNotification = this.notificationSource.asObservable();

  defaultNotificationArray: Notifications[];
  private notificationsSource = new BehaviorSubject(this.defaultNotificationArray);
  currentNotificationList = this.notificationsSource.asObservable();


  defaultEntreatyArray: Entreaty[];
  private entreatySource = new BehaviorSubject(this.defaultEntreatyArray);
  currentEntreatyList = this.entreatySource.asObservable();

  defaultIsUnread: boolean;
  private isUnreadSource = new BehaviorSubject(this.defaultIsUnread);
  isUnread = this.isUnreadSource.asObservable();


  constructor(private entreaty: NotificationsService) { }

  changeNotification(givenNotification: Entreaty) {
    this.notificationSource.next(givenNotification);
  }

  changeStudentEntreatyList() {
    this.entreaty.getStudentEntreties().subscribe(data => {
      let entreaties = data;
          entreaties = Logic.mapInfo(entreaties);
          entreaties = entreaties.filter( (val) => {
        if (val.entreatyType === 1) {
          return val;
        }
      });
      this.entreatySource.next(entreaties);
    });
  }


  changeReadStatus() {
   this.entreaty.getUnreadNotifications().subscribe(data => {
      this.isUnreadSource.next(data.length > 0);
      console.log(data.length > 0);
    });
  }

  changeNotificationList() {
    let givenNotificationList: Notifications[] = [];
    this.entreaty.getPastWeekNotifications().subscribe(data => {
      console.log(data);

      givenNotificationList = data;
      givenNotificationList = givenNotificationList.sort(function compare(a, b) {
        const dateA = new Date(a.time);
        const dateB = new Date(b.time);
        return +(dateB) - +(dateA);
      });
      this.notificationsSource.next(givenNotificationList);
    });
  }

}
