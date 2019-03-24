import { Component, OnInit } from '@angular/core';
import { NotificationCommunicationService } from '../services/notification-communication.service';
import { Entreaty } from '../models/entreaty.model';
import { GroupService } from '../services/group.service';
import { ComponentCommunicationService } from '../services/component-communication.service';
import { Router } from '@angular/router';
import { NotificationsService } from '../services/notifications.service';
import { StudentService } from '../services/student.service';

@Component({
  selector: 'app-notifications-body',
  templateUrl: './notifications-body.component.html',
  styleUrls: ['./notifications-body.component.css']
})
export class NotificationsBodyComponent implements OnInit {
  message: any;
  selectedNotification: Entreaty;

  constructor(
    private notify: NotificationCommunicationService,
    private groupService: GroupService,
    private studentService: StudentService,
    private commsService: ComponentCommunicationService,
    private _router: Router,
    private entrapy: NotificationsService
    ) {}

    display: boolean;

    ngOnInit() {
      this.notify.currentNotification.subscribe(data => {
        this.selectedNotification = data;
        console.log(data);
        // if (typeof data.id !== 'undefined') {
        //   this.display = false;
        // } else {
        //   this.display = true;
        // }
      });
      // this.entrapy.getPastWeekNotifications().subscribe(data => console.log(data));
    }

    view(groupId) {
      this.groupService.getGroup(groupId).subscribe(data => {
        const group = data;
        this.commsService.changeGroup(group);
        this.commsService.viewingProject = true;
        this._router.navigate(['/viewGroup']);
      });
    }
    accept(groupId: string, studentId: string) {
      this.entrapy.accept(groupId, studentId).subscribe(data => {
        this.notify.changeNotification(null);
        this.notify.changeStudentEntreatyList();
        this.studentService
        .getStudent(studentId)
        .subscribe(studentData => {
          this.studentService.changeStudent(studentData);
          this.groupService.getGroup(studentData.groupId).subscribe(groupData => {
            this.groupService.changeGroup(groupData);
            console.log(groupData);
          });
        });
      });
    }
    reject(groupId: string, studentId: string) {
      this.entrapy.reject(groupId, studentId).subscribe(data => {
        this.notify.changeNotification(null);
        this.notify.changeStudentEntreatyList();
      });
    }
  }
