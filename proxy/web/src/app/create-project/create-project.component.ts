import { Component, OnInit, OnDestroy } from '@angular/core';
import { ProjectService } from '../services/project.service';
import { ComponentCommunicationService } from '../services/component-communication.service';
import { Router } from '@angular/router';
import { NotificationsService } from '../services/notifications.service';
import { GroupService } from '../services/group.service';
import { AuthService } from '../services/auth.service';
import { Project } from '../models/project.model';
import { StudentService } from '../services/student.service';
import { MatSnackBar } from '@angular/material';


@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.css']
})
export class CreateProjectComponent implements OnInit, OnDestroy {

  isProposedProject: boolean;
  isEdit: boolean;
  project = {
    projectName: '',
    clientName: '',
    clientEmail: '',
    description: '',
    clientContact: '',
    ipType: 0,
    difficulty: 0
  };
  stream: any;

  constructor(private projectService: ProjectService, private route: Router,
    private comm: ComponentCommunicationService,
    private groupService: GroupService,
    private studentService: StudentService,
    private auth: AuthService,
    private notificationService: NotificationsService,
    public snackBar: MatSnackBar) { }


    ngOnInit() {
      if (this.comm.projectIdea.clientName !== '') {
        this.project.clientName = this.comm.projectIdea.clientName;
        this.project.clientEmail = this.comm.projectIdea.clientEmail;
        this.project.clientContact = this.comm.projectIdea.clientContactName;
        this.isProposedProject = true;
      } else if (this.comm.editingProposedProject) {
        this.comm.editProposedProject.subscribe(data => {
          this.project.projectName = String(data.projectName);
          this.project.description = String(data.description);
          this.project.clientName = String(data.clientName);
          this.project.clientEmail = String(data.clientEmail);
          this.project.clientContact = String(data.clientContact);
          this.project.ipType = data.ipType;
          this.project.difficulty = data.difficulty;
          this.isProposedProject = true;
          this.isEdit = true;
        });
      }
    }

    addProject() {
      this.openSnackBar(this.project.clientName);
      this.projectService.createProject(this.project).subscribe(data => {
        console.log(data);
        // this.route.navigate(['/home-group']);
        this.project = {
          projectName: '',
          clientName: '',
          clientEmail: '',
          description: '',
          clientContact: '',
          ipType: 0,
          difficulty: 0
        };
      });

      // this.addAllProjects();
    }

    addProjectProposed() {
      this.projectService.createProject(this.project).subscribe(data => {
        console.log(data);
        // this.route.navigate(['/home-group']);
        this.project = {
          projectName: '',
          clientName: '',
          clientEmail: '',
          description: '',
          clientContact: '',
          ipType: 0,
          difficulty: 0
        };
        this.stream = this.studentService.currentStudent
        .subscribe(groupData =>
          this.groupService.getGroup(groupData.groupId)
          .subscribe(groupDisplayData => {
            this.groupService.changeGroup(groupDisplayData);
            this.route.navigate(['/homeGroup']);
          }));
        });
      }

      editProject() {
        const project: Project = new Project();
        project.projectName = (this.project.projectName);
        project.description = (this.project.description);
        project.clientName = (this.project.clientName);
        project.clientEmail = (this.project.clientEmail);
        project.clientContact = (this.project.clientContact);
        project.ipType = this.project.ipType;
        project.difficulty = this.project.difficulty;

        this.projectService.updateProject(project);
        this.stream = this.studentService.currentStudent
        .subscribe(groupData =>
          this.groupService.getGroup(groupData.groupId)
          .subscribe(groupDisplayData => {
            this.groupService.changeGroup(groupDisplayData);
            this.route.navigate(['/homeGroup']);
          }));
        }
        ngOnDestroy() {
          this.stream.unsubscribe();
        }

        openSnackBar(name: string) {
          this.snackBar
          .open(`${name} Project is added to list`, 'Close', {
            duration: 3000,
          });
        }
        // addAllProjects() {
        //   const projectList: any = {
        //       'Project': '',
        //       'Company': '',
        //       'IP Type': -1,
        //       'Difficulty': 0,
        //       'Sponsor': '',
        //       'Contact': ''

        //   };
        //   this.notificationService.getProjects
        //     .subscribe(arg => projectList = arg);


        // }

      }
