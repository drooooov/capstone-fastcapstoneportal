import { Student } from './student.model';
export class Group {
    id: string;
    name: string;
    description: string;
    status: string;
    programName: string;
    profilePic: string;
    memberIds: Array<string>;
    groupAdmin: string;
    assignedProjectId: string;
    preferredProjectIds: Array<string>;
    proposedProjectId: string;
    scmLink: string;

    constructor() {}
  }
