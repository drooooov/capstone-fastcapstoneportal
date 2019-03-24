import { Injectable } from '@angular/core';
import { Project } from '../models/project.model';
import { Observable, BehaviorSubject } from 'rxjs';
import { Student } from '../models/student.model';
import { Group } from '../models/group.model';

@Injectable({
  providedIn: 'root'
})
export class ComponentCommunicationService {

  constructor() { }

  projectIdea = {
    'clientName': '',
    'clientContactName': '',
    'clientEmail': ''
  };
  viewingStudent = false;
  viewingProject = false;
  editingProposedProject = false;

  defaultProject = new Project();
  private unApprovedProjectSource = new BehaviorSubject<Project>(this.defaultProject);
  viewUnApprovedProject = this.unApprovedProjectSource.asObservable();

  private proposedProjectSource = new BehaviorSubject<Project>(this.defaultProject);
  editProposedProject = this.proposedProjectSource.asObservable();


  defaultGroup = new Group();
  private groupSource = new BehaviorSubject<Group>(this.defaultGroup);
  viewGroup = this.groupSource.asObservable();


  defaultStudent = new Student();
  private studentSource = new BehaviorSubject<Student>(this.defaultStudent);
  viewStudent = this.studentSource.asObservable();


  private projectSource = new BehaviorSubject<Project>(this.defaultProject);
  viewProject = this.projectSource.asObservable();

  changeUnappProject(project: Project) {
    console.log('is working?');
    this.unApprovedProjectSource.next(project);
  }

  changeStudent(student: Student) {
    console.log('is working?');
    this.studentSource.next(student);
  }

  changeGroup(group: Group) {
    console.log('is working?');
    this.groupSource.next(group);
  }

  editStudentProposedProject(project: Project) {
    this.proposedProjectSource.next(project);
  }
  changeProject(project: Project) {
    console.log('is working?');
    this.projectSource.next(project);
  }
}
