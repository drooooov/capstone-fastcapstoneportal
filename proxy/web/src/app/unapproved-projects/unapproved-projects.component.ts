import { Component, OnInit } from '@angular/core';
import { Project } from '../models/project.model';
import { ProjectService } from '../services/project.service';
import { ComponentCommunicationService } from '../services/component-communication.service';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-unapproved-projects',
  templateUrl: './unapproved-projects.component.html',
  styleUrls: ['./unapproved-projects.component.css']
})
export class UnapprovedProjectsComponent implements OnInit {

  comment: String = '';
  studentProjects = new Array<Project>();
  selectedProject: Project = new Project();
  displayPlaceholder = true;

  constructor(private projectService: ProjectService,
     private commsService: ComponentCommunicationService,
     private auth: AuthService) { }

  ngOnInit() {
    this.projectService.getUnApprovedProjects().subscribe(data => {
      console.log(data);
      this.studentProjects = data;
    });
    this.commsService.viewUnApprovedProject.subscribe( data => {
      this.selectedProject = data;
      console.log(data);
      if (typeof data.projectName !== 'undefined' ) {
        this.displayPlaceholder = false;
      }
    });
  }

  approve(project: Project) {
    this.projectService.approveProject(project.id).subscribe(data => {
      console.log(data);
      this.projectService.getUnApprovedProjects().subscribe(projectData =>
        this.studentProjects = projectData);
        this.displayPlaceholder = true;

      });
    }

    reject(project: Project) {
      this.projectService.unApproveProject(project.id).subscribe(data => {
        console.log(data);
        this.projectService.getUnApprovedProjects().subscribe(projectData =>
          this.studentProjects = projectData);
          this.displayPlaceholder = true;

        });
      }
      cancel() {
        this.displayPlaceholder = true;
      }
    }
