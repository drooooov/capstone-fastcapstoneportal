import { Component, OnInit } from '@angular/core';
import { Group } from '../models/group.model';
import { GroupService } from '../services/group.service';
import { StudentService } from '../services/student.service';
import { Student } from '../models/student.model';
import { ComponentCommunicationService } from '../services/component-communication.service';
import { Router } from '@angular/router';
import { Project } from '../models/project.model';
import { ProjectService } from '../services/project.service';
import { Subject } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-home-group',
  templateUrl: './home-group.component.html',
  styleUrls: ['./home-group.component.css']
})
export class HomeGroupComponent implements OnInit {
  private ngUnsubscribe = new Subject();
  curr_student: Student;
  curr_group: Group;
  groupName: string;
  description: string;
  membersList: Student[] = [];
  proposedProject: Project;

  groupId: String;
  constructor(
    private auth: AuthService,
    private groupService: GroupService,
    private studentService: StudentService,
    private comm: ComponentCommunicationService,
    private commsService: ComponentCommunicationService,
    private route: Router,
    private projectService: ProjectService
    ) {}

    ngOnInit() {
      const projectId = '';
      this.studentService.currentStudent.subscribe(data => {
        console.log(data);
        this.curr_student = data;
      });
      this.groupService.currentGroup.subscribe(data => {
        this.curr_group = data;
        console.log(data);
        if (data !== null) {
          this.displayMembers();
          if (data.proposedProjectId !== null) {
            this.projectService.getProject(data.proposedProjectId).subscribe(project => {
              console.log(project);
              this.proposedProject = project;
            });
          } else {
            this.proposedProject = null;
          }
        }
      });
    }

    createGroup() {
      const group = {
        groupAdminId: this.curr_student.id,
        name: this.groupName
      };
      let newGroupId = '';
      // create group service call
      this.groupService.createGroup(group).subscribe(data => {
        console.log(data);
        newGroupId = data.groupId;
        this.groupName = '';
        this.studentService
        .getStudent(this.curr_student.id)
        .subscribe(studentData =>
          this.studentService.changeStudent(studentData)
          );
          this.groupService.getGroup(newGroupId).subscribe(groupUpdateData => {
            console.log(groupUpdateData);
            this.groupService.changeGroup(groupUpdateData);
            this.groupService.currentGroup.subscribe(extra => {
              this.curr_group = extra;
              // this.displayMembers();
            });
          });
        });
      }

      leaveGroup() {
        this.studentService
        .removeStudentFromGroup(this.curr_student.id, this.curr_group.id)
        .subscribe(data => console.log(data));
        this.render(null);
      }

      displayMembers() {
        this.membersList = [];
        this.curr_group.memberIds.forEach(x => {
          this.studentService
          .getStudent(x)
          .subscribe(data => {
            this.membersList.push(data);
            console.log(data);

          });
        });
      }

      changeAdmin(studentId: string) {
        this.studentService
        .changeGroupAdmin(this.curr_student.id, studentId)
        .subscribe(() => {
          this.groupService
          .getGroup(this.curr_group.id)
          .subscribe(groupData => this.render(groupData.id));
        });
      }

      removeMember(studentId: string) {
        this.studentService
        .removeStudentFromGroup(studentId, this.curr_group.id)
        .subscribe(() => {
          this.groupService
          .getGroup(this.curr_group.id)
          .subscribe(groupData => this.render(groupData.id));
        });
      }

      render(groupId: string) {
        this.studentService
        .getStudent(this.curr_student.id)
        .subscribe(data => this.studentService.changeStudent(data));
        if (groupId === null) {
          this.groupService.changeGroup(null);
        } else {
          this.groupService.getGroup(groupId).subscribe(data => {
            console.log(data);
            this.groupService.changeGroup(data);
            console.log(this.curr_group);
          });
        }
      }

      createProject() {
        this.comm.projectIdea.clientContactName =
        this.curr_student.firstName + ' ' + this.curr_student.lastName;
        this.comm.projectIdea.clientEmail = this.curr_student.email;
        this.comm.projectIdea.clientName = this.curr_group.name;
        this.route.navigate(['/createProject']);
      }

      editProject() {
        this.projectService.getProject(this.curr_group.proposedProjectId).subscribe(data => {
          this.commsService.editStudentProposedProject(data);
          this.commsService.editingProposedProject = true;
          this.route.navigate(['/createProject']);
        });
      }
    }
