import { Component, OnInit } from '@angular/core';
import { Group } from '../models/group.model';
import { GroupService } from '../services/group.service';
import { NotificationsService } from '../services/notifications.service';
import { StudentService } from '../services/student.service';
import { Student } from '../models/student.model';


@Component({
  selector: 'app-group-looking',
  templateUrl: './group-looking.component.html',
  styleUrls: ['./group-looking.component.css']
})
export class GroupLookingComponent implements OnInit {

  //  writeJsonFile = require('write-json-file');

  groupList: Group[];
  notificationsList: any[];
  searchText: string;
  searchMemberNumber: number;
  curr_student: Student;
  groupsRequested: String[];

  isAvailable = true;
  // btnRequest(id: String) {
  //   // this.notificationService.getNotifications().subscribe(
  //   //   data => {
  //   //     this.notificationsList = data;


  //   //     // writeJsonFile('foo.json', ).then(() => {
  //   //       console.log('done');
  //   //     });
  //   //   }
  //   // );
  // }



  constructor(private studentsService: StudentService, private groupService: GroupService,
    private notifyService: NotificationsService) { }


  ngOnInit() {
    this.studentsService.currentStudent.subscribe(data =>{
      this.curr_student = data;
      console.log(data);
      this.notifyService.getStudentEntreties().subscribe(entreatyData => {
        this.groupsRequested = entreatyData.map(notify => {
          const groupId = notify.group;
          return groupId;
        });
        console.log(entreatyData);


      });
    });

    this.isAvailable = !(this.curr_student.groupId === null);
    console.log('isAvailable: - ', this.isAvailable);

    this.groupService.getGroups().subscribe(data => {
      console.log(data);
      this.groupList = data;
      console.log(this.groupList[0].memberIds);
    }, (err) => { },
      () => { });

      console.log(this.searchText);
      console.log('number: ' + this.searchMemberNumber);

  }

  addStudentToGroup(group: Group) {
    this.notifyService.request(group , this.curr_student).subscribe(data => {
        console.log(data);
        // this.studentsService.getStudents().subscribe(studentsData => this.students = studentsData);
        this.studentsService.getStudent(this.curr_student.id)
        .subscribe(studentData => this.studentsService.changeStudent(studentData));
      });

      this.isAvailable = false;

      this.groupService.getGroups().subscribe(data => this.groupList = data);
  }

  search() {

    console.log(this.searchText);
    console.log('number: ' + this.searchMemberNumber);

  }

}
