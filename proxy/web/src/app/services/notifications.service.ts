import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Entreaty } from '../models/entreaty.model';
import { Group } from '../models/group.model';
import { Student } from '../models/student.model';
import { UpperCasePipe } from '@angular/common';
import { Notifications } from '../models/notifications.model';

@Injectable()
export class NotificationsService {
  private _urlNotify: String = '/api/Notification';
  private _urlEntreaty: String = '/api/Entreaty';


  constructor(private http: HttpClient) { }



  getNotifications(): Observable<any[]> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    return this.http.get<any[]>(this._urlNotify + 'notifications.json');
  }

  getStudentEntreties(): Observable<Entreaty[]> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    return this.http.get<Entreaty[]>(this._urlEntreaty + '/GetStudentEntreaties', {headers: headers});
  }
  getGroupEntreties(): Observable<Entreaty[]> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    return this.http.get<Entreaty[]>(this._urlEntreaty + '/GetGroupEntreaties', {headers: headers});
  }

  getAllAnnouncements(): Observable<Notifications[]> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    return this.http.get<Notifications[]>(this._urlNotify + '/GetAllAnnouncementsAsync', {headers: headers});
  }

  invite(group: Group, student: Student, groupAdmin: Student) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    const message = `
    <p>You have received invite request from ${group.name} by ${groupAdmin.firstName}.
    Press View to view more information.</p>
    <br/>
    <p>To Join the team press accept or press decline to reject the invite.</p>
    `;
    console.log(message);
    return this.http.post(this._urlEntreaty + `/CreateInviteAsync/${student.id}`, `\"${message}\"`, {headers: headers});
  }

  request(group: Group, student: Student) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    const message = `
    <p>${student.firstName} requested to join your group</p>`;

    return this.http.post(this._urlEntreaty + `/CreateRequestAsync/${group.id}`, `\"${message}\"`, {headers: headers});
  }

  accept(groupId, studentId) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.put(`${this._urlEntreaty}/AcceptEntreaty/${groupId}?studentId=${studentId}`, null, {headers: headers});
  }

  reject(groupId, studentId) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.put(`${this._urlEntreaty}/RejectEntreaty/${groupId}?studentId=${studentId}`, null, {headers: headers});
  }

  getPastWeekNotifications(): Observable<Notifications[]> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    return this.http.get<Notifications[]>(this._urlNotify + '/GetAllPastWeeksNotifications/2', {headers: headers});
  }

  createAnnouncement(message: string) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.post(this._urlNotify + `/MakeAnnouncement`, `\"${message}\"`, {headers: headers});
  }

  readNotifications(notifications: String[] ) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.put(`${this._urlNotify}/MarkNotificationsAsRead`, notifications, {headers: headers});
  }

  getUnreadNotifications() {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    return this.http.get<Notifications[]>(this._urlNotify + '/GetAllUnreadNotifications/2', {headers: headers});
  }

}
