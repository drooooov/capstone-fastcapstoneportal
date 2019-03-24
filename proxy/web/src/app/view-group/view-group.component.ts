import { Component, OnInit } from '@angular/core';
import { ComponentCommunicationService } from '../services/component-communication.service';
import { Router } from '@angular/router';
import { Group } from '../models/group.model';
import { Student } from '../models/student.model';
import { StudentService } from '../services/student.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-view-group',
  templateUrl: './view-group.component.html',
  styleUrls: ['./view-group.component.css']
})
export class ViewGroupComponent implements OnInit {

  curr_group: Group;
  membersList: Student[] = [];

  constructor( private studentService: StudentService,
    private commsService: ComponentCommunicationService,
    private _router: Router,
    private _location: Location) { }

    ngOnInit() {
      this.commsService.viewGroup.subscribe(data => {
        this.curr_group = data;
        this.displayMembers();
        console.log(this.curr_group);
      });
    }

    displayMembers() {
      this.membersList = [];
      this.curr_group.memberIds.forEach(x => {
        this.studentService
        .getStudent(x)
        .subscribe(data => this.membersList.push(data));
      });
    }
    cancel() {
      console.log('working');
      this._location.back();
    }
  }
