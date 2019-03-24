import { Component, OnInit } from '@angular/core';
import { StudentService } from '../services/student.service';
import { Student } from '../models/student.model';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import {COMMA, ENTER} from '@angular/cdk/keycodes';
import {MatChipInputEvent} from '@angular/material';

export interface Skill {
  name: string;
}

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {

  img_src = '';
  visible = true;
  selectable = true;
  removable = true;
  addOnBlur = false;
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];
  skills: Skill[] = [
    {name: 'Lemon'},
    {name: 'Lime'},
    {name: 'Apple'},
  ];
  constructor(private studentService: StudentService, private auth: AuthService, private _router: Router) { }

  currentStudent: Student = new Student();
  ngOnInit() {
    this.studentService.currentStudent.subscribe(student => {
      this.currentStudent = student;
      this.skills = this.currentStudent.skills.map(val => {
        return { 'name': val};
      });
      console.log(this.skills);

      if (typeof this.currentStudent.picture === 'undefined') {
        this.img_src = '../../assets/images/businessman.png';
      } else {
        this.img_src = `http://3.16.54.7/Images/${this.currentStudent.picture}`;
      }
    });
  }

  add(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;

    // Add our skill
    if ((value || '').trim()) {
      this.skills.push({name: value.trim()});
    }

    // Reset the input value
    if (input) {
      input.value = '';
    }
  }

  remove(skill: Skill): void {
    const index = this.skills.indexOf(skill);

    if (index >= 0) {
      this.skills.splice(index, 1);
    }
  }


  updateStudent() {
    this.currentStudent.skills = this.skills.map(obj => {
      return obj.name;
    });
    console.log(this.currentStudent);

    this.studentService.updateStudent(this.currentStudent).subscribe(data => {
      console.log(data);
      this.studentService.changeStudent(this.currentStudent);
      this._router.navigate(['/profile']);

    });
    // ,
    // (err)=> {
    //   if(err.)
    //   this.auth.refreshToken()
    // }
  }

  onFileChanged(event) {
    const file = event.target.files[0];
    console.log(file);
    this.upload(file);
  }

  upload(file: any) {
    console.log(file);

    this.studentService.uploadImage(file, +this.currentStudent.id).subscribe(data => {

     console.log(data);
     this.currentStudent.picture = data.toString();
     this.img_src = `http://3.16.54.7/Images/${this.currentStudent.picture}`;
    });
  }
}
