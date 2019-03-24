import { ProjectService } from './../services/project.service';
import { GroupService } from './../services/group.service';
import { Group } from './../models/group.model';
import { Component, OnInit } from '@angular/core';
import { stringify } from '@angular/core/src/render3/util';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-rank-projects',
  templateUrl: './rank-projects.component.html',
  styleUrls: ['./rank-projects.component.css']
})
export class RankProjectsComponent implements OnInit {

  curr_group: Group;
  preferredProjects: any = [];
  constructor(private projectService: ProjectService,
    private groupService: GroupService,
    public snackBar: MatSnackBar) { }

  ngOnInit() {
    this.groupService.currentGroup.subscribe(data => {
      this.curr_group = data;
      this.getProjectPreferences();
    });
  }

  getProjectPreferences() {
    this.groupService.getProjectsWithPreference(this.curr_group.id).subscribe(data => {
      this.preferredProjects = data.map((v, i) => {
      return {
        'projectId': v.id,
        'projectName': v.projectName,
        'company': v.clientName,
        'rank': (i + 1)
      };
    });

    console.log(this.preferredProjects);
  });
}

  changeRank(direction: string, rank: number) {
      if (direction === 'up') {
        if (rank !== 1) {
          const temp = this.preferredProjects[rank - 1];
          this.preferredProjects[rank - 1] = this.preferredProjects[rank - 2];
          this.preferredProjects[rank - 2] = temp;
          this.preferredProjects[rank - 1 ].rank = rank;
          this.preferredProjects[rank - 2].rank = rank - 1;
        }
      } else {
        if (rank !== 5) {
          const temp = this.preferredProjects[rank - 1];
          this.preferredProjects[rank - 1] = this.preferredProjects[rank];
          this.preferredProjects[rank] = temp;
          this.preferredProjects[rank - 1].rank = rank;
          this.preferredProjects[rank].rank = rank + 1;
        }
      }
      console.log(this.preferredProjects);
    }

    rankProjects() {
      const arr = this.preferredProjects.reduce(function (map, obj) {
        map[obj.rank] = obj.projectId;
        return map;
      }, {});
      this.groupService.rankProjects(this.curr_group.id, arr).subscribe(data => {
        console.log(data);
        this.openSnackBar();
      });
    }

    openSnackBar() {
      this.snackBar.open('Projects ranking has been updated', 'Close', {
        duration: 3000,
      });
    }
}
