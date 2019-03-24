import { Group } from './group.model';
export class Project {
  id: string;
  projectName: String;
  description: string;
  difficulty: number;
  ipType: number;
  approved: boolean;
  clientName: string;
  clientEmail: string;
  clientContact: string;
  comments: string;
  preferredByGroupIds: number[];
  ipTypeLiteral: string;
  constructor();
  constructor(projectName?, description?, difficulty?, ipType?, clientName?, clientEmail?) {
    this.projectName = projectName;
    this.description = description;
    this.difficulty = difficulty;
    this.ipType = ipType;
    this.clientName = clientName;
    this.clientEmail = clientEmail;
  }

}
