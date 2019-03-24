import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Project } from '../models/project.model';

@Injectable()
export class ProjectService {
  constructor(private http: HttpClient) { }

  projectUrl = '/api/Projects';

  getProjects(): Observable<Project[]> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    return this.http.get<Project[]>(this.projectUrl + '/GetAllProjects', {headers: headers});
  }

  getProject(projectId: string) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    return this.http.get<Project>(this.projectUrl + `/GetProjectById/${projectId}`, {headers: headers});
  }

  createProject(project: any): any {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    return this.http.post(this.projectUrl + `/CreateProject`, project, {headers: headers});
  }

  getApprovedProjects(): Observable<Project[]> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    return this.http.get<Project[]>(this.projectUrl + '/GetApprovedProjects', {headers: headers});
  }
  getUnApprovedProjects(): Observable<Project[]> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    return this.http.get<Project[]>(this.projectUrl + '/GetUnapprovedProjects', {headers: headers});
  }
  approveProject(projectId: string): any {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.put(`${this.projectUrl}/ApproveProject/${projectId}`, {}, {headers: headers});
  }

  unApproveProject(projectId: string): any {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.put(`${this.projectUrl}/UnApproveProject/${projectId}`, {}, {headers: headers});
  }

  updateProject(project: Project): any {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.put(`${this.projectUrl}/UpdateProject/${project.id}`, project, {headers: headers});
  }

  deleteProject(projectID: string) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.delete(`${this.projectUrl}/DeleteProject/${projectID}`, {headers: headers});
  }
}
