import { Component, OnInit, OnDestroy } from '@angular/core';
import { ProjectService } from '../services/project.service';
import { ComponentCommunicationService } from '../services/component-communication.service';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { Project } from '../models/project.model';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-project-info',
  templateUrl: './project-info.component.html',
  styleUrls: ['./project-info.component.css']
})
export class ProjectInfoComponent implements OnInit, OnDestroy {

  constructor(private projectService: ProjectService,
    private commsService: ComponentCommunicationService,
    private _router: Router,
    private _location: Location,
    private auth: AuthService) { }
  currentProject: Project;
  viewingProject = false;
  isAdmin: string;

  ngOnInit() {
    this.isAdmin = localStorage.getItem('isAdmin');
    if (this.commsService.viewingProject) {
      this.commsService.viewProject.subscribe(data => {
        this.currentProject = data;
        switch (this.currentProject.ipType) {
          case 0: this.currentProject.ipTypeLiteral = 'Open'; break;
          case 1: this.currentProject.ipTypeLiteral = 'Free Licence'; break;
          case 2: this.currentProject.ipTypeLiteral = 'Joint Venture'; break;
          case 3: this.currentProject.ipTypeLiteral = 'Client'; break;
        }
      });
      this.viewingProject = this.commsService.viewingProject;
      console.log(this.viewingProject);
    } else {
      // this.studentService.currentStudent.subscribe(data => this.currentStudent = data);
      // console.log(this.currentStudent);
    }
  }

  cancel() {
    console.log('working');
    this._location.back();
  }

  delete() {
    this.projectService.deleteProject(this.currentProject.id).subscribe(data => {
      console.log(data);
      this.cancel();
    });
  }
  ngOnDestroy() {
    this.commsService.viewingProject = false;
    this.viewingProject = false;
  }

}
