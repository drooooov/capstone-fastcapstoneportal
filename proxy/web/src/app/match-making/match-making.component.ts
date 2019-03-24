import { Project } from './../models/project.model';
import { Group } from './../models/group.model';
import { Component, OnInit } from '@angular/core';
import { GroupService } from '../services/group.service';
import { StudentService } from '../services/student.service';
import { ProjectService } from '../services/project.service';

@Component({
  selector: 'app-match-making',
  templateUrl: './match-making.component.html',
  styleUrls: ['./match-making.component.css']
})
export class MatchMakingComponent implements OnInit {


  show = false;
  groupList: Group[];
  projectList: Project[];
  halfValue: number;
  displayResults = false;
  displayList: any[] = [];


  constructor(private groupService: GroupService,
    private studentService: StudentService,
    private projectService: ProjectService  ) { }

    ngOnInit() {
      this.groupService.getGroups().subscribe(data => {
        this.groupList = data;
        console.log(data);
      });
      this.projectService.getProjects().subscribe(data => {

        this.projectList = data;
        console.log(data);
      });
    }


    display() {
      this.show = !this.show;
    }

    match() {
      console.log('running?');

      this.groupService.matchProjects().subscribe(data => console.log(data));
      this.groupService.getGroups().subscribe(data => {
        this.groupList = data.map(v => {
          if (v.assignedProjectId !== null) {
            return v;
          }
        });
        this.groupList = this.groupList.sort((a: Group, b: Group) => {
          return (+a.assignedProjectId) - (+b.assignedProjectId);
        });
        console.log(this.groupList);

        let selectedProjectList = data.map(v => {
          return v.assignedProjectId;
        });
        selectedProjectList = selectedProjectList.sort(function(a: any, b: any) {
          return a - b;
        });
        console.log(selectedProjectList);

        const selectedProjectFullList = this.projectList.filter(v => {
          if (selectedProjectList.includes(v.id)) {
            return {
              'projectId': v.id,
              'projectName': v.projectName,
              'company': v.clientName
            };
          }
        });
        console.log(selectedProjectFullList);


        for (let i = 0; i < selectedProjectFullList.length; i++) {
          if (selectedProjectFullList[i].id === this.groupList[i].assignedProjectId) {

            this.displayList.push ({
              'projectName': selectedProjectFullList[i].projectName,
              'company': selectedProjectFullList[i].clientName,
              'groupName': this.groupList[i].name
            });
          }
        }
        console.log(this.displayList);

        this.projectList = this.projectList.filter(v => {
          if (!selectedProjectList.includes(v.id)) {
            return v;
          }
        });
        console.log(this.projectList);

        this.displayResults = true;
      });
    }

  }
