import { Component, OnInit, Input } from '@angular/core';
import { Project } from '../models/project.model';
import { ProjectService } from '../services/project.service';
import { ComponentCommunicationService } from '../services/component-communication.service';

@Component({
  selector: 'app-unapproved-projects-list',
  templateUrl: './unapproved-projects-list.component.html',
  styleUrls: ['./unapproved-projects-list.component.css']
})
export class UnapprovedProjectsListComponent implements OnInit {

  @Input() projectList: Project[];
  constructor(private projectService: ProjectService, private commsService: ComponentCommunicationService) { }

  ngOnInit() {}

  selectProject(givenProject: Project) {
    this.commsService.changeUnappProject(givenProject);
  }

}
