import { Component, OnInit } from '@angular/core';
import { Student } from '../models/student.model';
import { Group } from '../models/group.model';
import { GroupService } from '../services/group.service';
import { StudentService } from '../services/student.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-edit-group',
  templateUrl: './edit-group.component.html',
  styleUrls: ['./edit-group.component.css']
})
export class EditGroupComponent implements OnInit {

  curr_student: Student;
  curr_group: Group;
  groupName: string ;
  description: string;
  membersList: Student[] = [];


  constructor(private groupService: GroupService,
    private studentService: StudentService, private _router: Router) { }


    ngOnInit() {
      this.studentService.currentStudent.subscribe(data => this.curr_student = data);
      this.groupService.currentGroup.subscribe(data => {
        this.curr_group = data;
        this.groupName = this.curr_group.name;
        this.description = this.curr_group.description;
        this.displayMembers();
      });
    }
    displayMembers() {
      this.membersList = [];
      this.curr_group.memberIds.forEach((x, i, arr) => {
        this.studentService.getStudent(x).subscribe(data => this.membersList.push(data));
      });
    }

    update() {
      const updatedGroup = this.curr_group;
      updatedGroup.description = this.description;
      updatedGroup.name = this.groupName;
      this.groupService.updateDescription(updatedGroup)
        .subscribe(arg => console.log(arg));
      this.groupService.updateName(updatedGroup)
        .subscribe(arg => {
          console.log(arg);
          this.groupService.changeGroup(updatedGroup);
          this._router.navigate(['/homeGroup']);
        });
    }


}
