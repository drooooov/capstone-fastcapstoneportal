import { Observable, BehaviorSubject } from 'rxjs';
import { Injectable } from '@angular/core';
import { Student } from '../models/student.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { utils } from 'protractor';

@Injectable()
export class StudentService {

  private _url1 = '/api/Students';


  private studentSource = new BehaviorSubject(new Student());
  currentStudent = this.studentSource.asObservable();

  constructor(private http: HttpClient) { }

  changeStudent(student: Student) {
    this.studentSource.next(student);
  }


  getStudents(): Observable<Student[]> {
    const headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': 'Bearer ' + localStorage.getItem('token')
  });
    return this.http.get<Student[]>(this._url1 + '/GetAllStudents', {headers: headers});
  }



  getStudent(studentId: string): Observable<Student> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    return this.http.get<Student>(`/api/Students/GetStudentById/${studentId}`, {headers: headers});
  }

  updateStudent(givenStudent: Student): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    const student: any = {
      'firstName': givenStudent.firstName,
      'lastName': givenStudent.lastName,
      'picture': givenStudent.picture,
      'campus': true,
      'description': givenStudent.description,
      'linkedinLink': givenStudent.linkedinLink,
      'portfolioLink': givenStudent.portfolioLink,
      'role': givenStudent.role,
      'skills': givenStudent.skills
    };
    console.log(student);
    console.log(headers);
    return this.http.put(`/api/Students/UpdateStudent/${givenStudent.id}`, student, {headers: headers});
  }
  getStudentsByProgram (programName: string): Observable<Student[]> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    return this.http.get<Student[]>(`/api/Students/GetStudentByProgram/${programName}`, {headers: headers});
  }

  addStudentToGroup(studentId: string, groupId: string): any {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.put(this._url1 + `/PutStudentInGroup/${studentId}?groupId=${groupId}`, {'groupId': groupId}, {headers: headers});
  }

  removeStudentFromGroup(studentId: string, groupId: string): any {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.put(this._url1 + `/RemoveStudentFromGroup/${studentId}?groupId=${groupId}`, {'groupId': groupId}, {headers: headers});
  }

  changeGroupAdmin(studentId: string, newAdminId: string): any {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });

    return this.http.put(this._url1 + `/ChangeGroupAdmin/${studentId}?newAdminId=${newAdminId}`, {}, {headers: headers});
  }

  uploadImage(file: any, studentId: number) {
    const headers = new HttpHeaders({
      'enctype': 'multipart/form-data',
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    const uploadData: FormData = new FormData();
    uploadData.append('file', file);

    return this.http.post(`/api/Image/UploadStudentImage/${studentId}`, uploadData, {headers: headers});

  }
}
