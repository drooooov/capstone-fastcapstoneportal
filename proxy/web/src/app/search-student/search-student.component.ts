import { Component, OnInit } from '@angular/core';
import { StudentService } from '../services/student.service';
import { Student } from '../models/student.model';
import { GroupService } from '../services/group.service';
import { Group } from '../models/group.model';
import { ComponentCommunicationService } from '../services/component-communication.service';
import { Router } from '@angular/router';
import { NotificationsService } from '../services/notifications.service';
import { createOfflineCompileUrlResolver } from '@angular/compiler';
import { CampusText } from '../enums/Campus.enum';

@Component({
  selector: 'app-search-student',
  templateUrl: './search-student.component.html',
  styleUrls: ['./search-student.component.css']
})
export class SearchStudentComponent implements OnInit {

  public studentsList: Student[] = [];
  students: Student[];
  invitedStudents: String[];
  curr_student: Student;
  curr_group: Group;
  CAMPUS_TEXT = CampusText;

  title: String = 'Search Student';
  constructor(private studentsService: StudentService, private groupService: GroupService,
    private commsService: ComponentCommunicationService, private _router: Router,
    private notifyService: NotificationsService) { }

  ngOnInit() {
    this.studentsService.currentStudent.subscribe(data => this.curr_student = data);
    this.groupService.currentGroup.subscribe(data => {
      this.curr_group = data;
      console.log(data);
      if (typeof data !== 'undefined') {
        this.notifyService.getGroupEntreties().subscribe(entreatyData => {
          this.invitedStudents = entreatyData.map(notify => {
            const studentId = notify.student;
            return studentId;
          });
          console.log(entreatyData);
        });
      }
    });
    this.studentsService.getStudents()
    .subscribe(data => {
      this.studentsList = data;
      // this.studentsList = this.studentsList.map( x => {
      //   if (x.campus == 'true') {
      //     x.campusLiterals = 'Davis';
      //   } else {
      //     x.campusLiterals = 'Trafalgar';
      //   }
      //   return x;
      // });
    }, (err) => {
      console.log(err);
    },
      () => {
        this.students = this.studentsList;
        console.log(this.students);
      }
    );
  }

  inviteStudent(student: Student) {
    this.notifyService.invite(this.curr_group, student, this.curr_student).subscribe(data => {
        console.log(data);
        this.studentsService.getStudents().subscribe(studentsData => this.students = studentsData);
        this.groupService.getGroup(this.curr_group.id)
        .subscribe(groupData => this.groupService.changeGroup(groupData));
      });
  }

  viewStudent(student: Student) {
    this.commsService.changeStudent(student);
    this.commsService.viewingStudent = true;
    this._router.navigate(['/profile']);
  }

}
