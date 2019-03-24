import { Group } from './../models/group.model';
import { GroupService } from './../services/group.service';
import { Component, OnInit } from '@angular/core';
import { ProjectService } from '../services/project.service';
import { Project } from '../models/project.model';
import { ComponentCommunicationService } from '../services/component-communication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-view-projects',
  templateUrl: './view-projects.component.html',
  styleUrls: ['./view-projects.component.css']
})
export class ViewProjectsComponent implements OnInit {

  projectList: Project[];
  addProjectList: any[];
  curr_group: Group;
  // preferredProjects: any = [
  //   {
  //     'projectId': '',
  //     'rank': 0
  //   }
  // ];
  preferredProjects = [];

  constructor(private projectService: ProjectService,
    private groupService: GroupService,
    private commsService: ComponentCommunicationService,
     private _router: Router) { }

  ngOnInit() {
    this.groupService.currentGroup.subscribe(data => {
      this.curr_group = data;
      console.log(data);
      this.getProjectPreferences();
    });

    this.projectService.getProjects().subscribe(data => {
      this.projectList = data;
      this.projectList = this.projectList.map(x => {
        switch (x.ipType) {
          case 0: x.ipTypeLiteral = 'Open'; break;
          case 1: x.ipTypeLiteral = 'Free Licence'; break;
          case 2: x.ipTypeLiteral = 'Joint Venture'; break;
          case 3: x.ipTypeLiteral = 'Client'; break;
        }
        return x;
      });

      console.log(this.projectList);

    });

  }

  addProjectToPreference(project: Project) {
    // this.preferredProjects.push(project);
    // this.preferredProjects.splice(this.preferredProjects.indexOf(project), 1);
    this.groupService.addProjectToPreference(this.curr_group.id, project.id).subscribe(data => {
      console.log(data);
      this.groupService.getGroup(this.curr_group.id).subscribe(groupData => {
        this.curr_group = groupData;
        this.groupService.changeGroup(groupData);
        this.getProjectPreferences();
      });


    });
  }

  getProjectPreferences() {
    this.groupService.getGroup(this.curr_group.id).subscribe(data => {
      this.preferredProjects = data.preferredProjectIds.map((v, i) => {
        return {
          'projectId': v,
          'rank': (i + 1)
        };
      });
      // this.addProjectList = this.projectList.map(x => {

      //   const obj = {
      //     'id': x.id,
      //     'show': data.preferredProjectIds.includes(x.id)
      //   };
      //   return obj;
      // });
      console.log(this.preferredProjects);
      // console.log(this.addProjectList);
  });
  }

  viewProject(project: Project) {
    this.commsService.changeProject(project);
    this.commsService.viewingProject = true;
    this._router.navigate(['/viewProject']);
  }

}
