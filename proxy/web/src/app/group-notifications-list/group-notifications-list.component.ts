import { Component, OnInit, Input } from '@angular/core';
import { NotificationType } from '../enums/NotificationType.enum';
import { NotificationColor } from '../enums/NotificationColor.enum';
import { NotificationCommunicationService } from '../services/notification-communication.service';
import { GroupService } from '../services/group.service';
import { ComponentCommunicationService } from '../services/component-communication.service';
import { Router } from '@angular/router';
import { NotificationsService } from '../services/notifications.service';
import { Notifications } from '../models/notifications.model';
import { Entreaty } from '../models/entreaty.model';
import { GroupNotifyTitle } from '../enums/GroupNotifyTitle.enum';
import { StudentService } from '../services/student.service';
import { Student } from '../models/student.model';

@Component({
  selector: 'app-group-notifications-list',
  templateUrl: './group-notifications-list.component.html',
  styleUrls: ['./group-notifications-list.component.css']
})
export class GroupNotificationsListComponent implements OnInit {

  @Input() isAdmin: boolean;
  notificationsList: Entreaty[];

  public NOTIFY_TYPE = [
    'Request', 'Invite'
  ];
  public NOTIFY_TITLE = GroupNotifyTitle;

  public NOTIFY_COLOR = NotificationColor;

  constructor(private notify: NotificationCommunicationService,
    private studentService: StudentService,
    private groupService: GroupService,
    private commsService: ComponentCommunicationService,
    private _router: Router,
    private entrapy: NotificationsService) { }

    ngOnInit() {
      this.getAllNotifications();
    }

    getAllNotifications() {
      this.entrapy.getGroupEntreties().subscribe(data => {
        console.log(data);
        this.notificationsList = data;
        this.notificationsList.map(val => {
          this.studentService.getStudent(val.student).subscribe(studentData => {
            val.message = GroupNotifyTitle[val.entreatyType].replace('Bla', studentData.firstName);
            console.log(val.message);
            return val;
          });
        });
      });
    }

    viewStudent(studentId: string) {
      this.studentService.getStudent(studentId).subscribe(studentData => {
        this.commsService.changeStudent(studentData);
        this.commsService.viewingStudent = true;
        this._router.navigate(['/profile']);
      });
    }

    accept(groupId: string, studentId: string) {
      this.entrapy.accept(groupId, studentId).subscribe(data => {
        console.log('working?');
        this.groupService.getGroup(groupId).subscribe(groupData => {
          this.groupService.changeGroup(groupData);
          console.log(groupData);
        });
        this.getAllNotifications();
      });
    }
    reject(groupId: string, studentId: string) {
      this.entrapy.reject(groupId, studentId).subscribe(data => {
        this.getAllNotifications();
      });
    }
  }
