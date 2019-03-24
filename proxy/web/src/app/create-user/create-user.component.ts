import { Component, OnInit } from '@angular/core';
import { User } from '../models/user.model';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
  styleUrls: ['./create-user.component.css']
})
export class CreateUserComponent implements OnInit {

  constructor(private userService: UserService) { }

  user: User = {
    FirstName: '',
    LastName: '',
    Username: '',
    Password: '',
    ProgramName: '-1',
    Email: '',
    isAdmin: false



  };


  ngOnInit() {
  }

adduser() {
  this.userService.creatUser(this.user).subscribe(data => console.log(data));

  console.log(this.user);
  this.user = {
    FirstName: '',
    LastName: '',
    Username: '',
    Password: 'Start?123',
    ProgramName: '-1',
    Email: '',
    isAdmin: false



  };
}

}
