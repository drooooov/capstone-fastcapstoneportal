import { Group } from './../models/group.model';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { Student } from '../models/student.model';

@Injectable()
export class GroupService {


  groupUrl = '/api/Groups';
  constructor(private http: HttpClient) { }
  defaultGroup: Group;
  private groupSource = new BehaviorSubject<Group>(new Group());
  currentGroup = this.groupSource.asObservable();


  changeGroup(group: Group) {
    console.log('is working?');
    this.groupSource.next(group);
  }

  getGroups(): Observable<Group[]> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.get<Group[]>(this.groupUrl + '/GetGroups', {headers: headers});
  }

  getGroup(groupId: string) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.get<Group>(this.groupUrl + `/GetGroupById/${groupId}`, {headers: headers});
  }

  createGroup(group: any): Observable<any> {
    const headers1 = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    const params = new URLSearchParams();
    params.set('groupAdminId', group.groupAdminId);

  console.log(params.toString());
  console.log(headers1);
  return this.http.post(`/api/Groups/CreateGroup/${group.name}?groupAdminId=${group.groupAdminId}`, null, { headers: headers1});
  }

  updateName(group: Group): any {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.put(`${this.groupUrl}/UpdateName/${group.id}`, {'name': group.name}, {headers: headers});
  }

  updateDescription(group: Group): any {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.put(`${this.groupUrl}/UpdateDescription/${group.id}`, {'description': group.description}, {headers: headers});
  }

  addProjectToPreference(groupId: string, projectId: string): Observable<any> {
    const headers1 = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    const params = new URLSearchParams();
    params.set('projectId', projectId);

  console.log(params.toString());
  console.log(headers1);
  return this.http.post(`/api/Groups/AddProjectToPreference/${groupId}?projectId=${projectId}`, null, { headers: headers1});
  }

  getProjectsWithPreference(groupId: string): Observable<any[]> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.get<any[]>(this.groupUrl + `/GetProjectPreferences/${groupId}`, {headers: headers});
  }

  matchProjects() {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.post(this.groupUrl + `/MatchProjects`, null, {headers: headers});
  }

  rankProjects(groupId: string, projects: any) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.post(this.groupUrl + `/RankProjects/${groupId}`, projects, {headers: headers});
  }
}
