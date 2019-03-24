import { Group } from './../models/group.model';
import { Student } from './../models/student.model';
import { GroupService } from './../services/group.service';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-members-display',
  templateUrl: './members-display.component.html',
  styleUrls: ['./members-display.component.css']
})
export class MembersDisplayComponent implements OnInit {

  @Input() groupID: String;
  memberList: string[];
  groupList: Group[];

  constructor(private groupService: GroupService) { }


  ngOnInit() {
    this.groupService.getGroups().subscribe(data => {
      this.groupList = data;
    }, (err) => { },
      () => {
        console.log('bla');
        this.groupList.forEach(element => {
          if (element.id === this.groupID) {

            this.memberList = element.memberIds;
            console.log(element);
          }
        });
      });

    console.log('bla');



  }

}
