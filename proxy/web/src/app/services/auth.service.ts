import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { StudentService } from './student.service';
import { GroupService } from './group.service';
import { SignalRServiceService } from './signal-rservice.service';
import { NotificationCommunicationService } from './notification-communication.service';

interface TokenData {
  access_token: string;
  refresh_token: string;
  roles: string[];
  expiration: string;
  id: number;
}


@Injectable()
export class AuthService {

  constructor(private http: HttpClient,
    private studentService: StudentService,
    private groupService: GroupService,
    private signalr: SignalRServiceService,
    private notify: NotificationCommunicationService,
    private _router: Router) {
      const date = new Date();
      console.log(date);
      if (localStorage.getItem('expires_at') !== (null || '')) {

        const expiringDate = new Date(localStorage.getItem('expires_at'));
        // If app auth token is not expired, request new token
        if ( expiringDate.toISOString() > date.toISOString()) {
          console.log(new Date(localStorage.getItem('expires_at')));
          this.refreshToken();
        }
      }
    }

    loginUrl = '/api/Account';

    loggedIn = false;
    private loginSource = new BehaviorSubject(this.loggedIn);
    isLoggedIn = this.loginSource.asObservable();

    changeLoginStatus(input: boolean) {
      this.loginSource.next(input);
      this.loggedIn = input;
    }

    logoutUser() {
      localStorage.removeItem('token');
      this._router.navigate(['/login']);
    }

    getToken() {
      return localStorage.getItem('token');
    }

    get tokenValid(): boolean {
      const date = new Date();
      console.log(date);
      if (localStorage.getItem('expires_at') !== (null || '')) {
        const expiringDate = new Date(localStorage.getItem('expires_at'));
      // Check if current time is past access token's expiration

      return expiringDate.toISOString() > date.toISOString();
      }
    }

    handleAuth(data: TokenData) {
      localStorage.setItem('token', data.access_token);
      localStorage.setItem('refresh-token', data.refresh_token);
      localStorage.setItem('isAdmin', '' + data.roles.includes('Admin'));
      localStorage.setItem('id', data.id + '');
      console.log(new Date(data.expiration));
      localStorage.setItem('expires_at', data.expiration);
      this.changeLoginStatus(true);
    }

    logInStudent() {
      const id = localStorage.getItem('id');
      this.studentService.getStudent(id + '').subscribe(
        student => {
          this.studentService.changeStudent(student);
          // tslint:disable-next-line:triple-equals
          if (student.groupId != ('' || null)) {
            console.log(student.groupId);
            this.groupService.getGroup(student.groupId)
            .subscribe(groupData => this.groupService.changeGroup(groupData));
          } else {
            this.groupService.changeGroup(null);
          }
        });
        this.notify.changeReadStatus();
      }

      login(user: any) {
        return this.http.post<TokenData>(this.loginUrl + '/Login', user);
      }

      refreshToken() {

        const tokenObj = {
          'access_token': `${localStorage.getItem('token')}`,
          'refresh_token': `${localStorage.getItem('refresh-token')}`
        };
        console.log(tokenObj);

        return this.http.post<TokenData>(this.loginUrl + '/Refresh', tokenObj).subscribe(data => {
          localStorage.setItem('token', data.access_token);
          localStorage.setItem('refresh-token', data.refresh_token);
          localStorage.setItem('isAdmin', '' + data.roles.includes('Admin'));
          localStorage.setItem('id', data.id + '');
          console.log(new Date(data.expiration));
          localStorage.setItem('expires_at', data.expiration);
          this.changeLoginStatus(true);
          if (JSON.parse(localStorage.isAdmin) === false) {
            this.logInStudent();
          }
        });
      }
    }
