import { StudentService } from './../services/student.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Student } from '../models/student.model';
import { ComponentCommunicationService } from '../services/component-communication.service';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { ProgramType } from '../enums/Program.enum';
import {MatChipsModule} from '@angular/material/chips';


@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit, OnDestroy {

  public studentsList: Student[] = [];
  public currentStudent: Student;
  viewingStudent = false;
  isdataLoaded = false;
  // program = '';
  email = '';
  site = '';
  skills = [];
  constructor(private studentService: StudentService,
    private commsService: ComponentCommunicationService,
    private _router: Router,
    private _location: Location) { }

    img_src = '';
    PROGRAM_TEXT = ProgramType;
    ngOnInit() {
      if (this.commsService.viewingStudent) {
        this.commsService.viewStudent.subscribe(data => {
          this.currentStudent = data;
          if ( this.currentStudent.picture === null) {
            this.img_src = '../../assets/images/businessman.png';
          } else {
            this.img_src = `http://3.16.54.7/Images/${this.currentStudent.picture}`;
          }
          this.skills = this.currentStudent.skills.map(val => {
            return { 'name': val};
          });
        });
        this.viewingStudent = this.commsService.viewingStudent;
      } else {
        this.studentService.currentStudent.subscribe(data => {
          this.currentStudent = data;
          if ( this.currentStudent.picture === null)  {
            this.img_src = '../../assets/images/businessman.png';
          } else {
            this.img_src = `http://3.16.54.7/Images/${this.currentStudent.picture}`;
          }
          this.skills = this.currentStudent.skills.map(val => {
            return { 'name': val};
          });
        });
        console.log(this.currentStudent);
      }
    }

    cancel() {
      console.log('working');
      this._location.back();
    }
    ngOnDestroy() {
      this.commsService.viewingStudent = false;
      this.viewingStudent = false;
    }
  }
