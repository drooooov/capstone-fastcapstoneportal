import { Component, OnInit, AfterViewChecked } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Student } from '../models/student.model';
import { StudentService } from '../services/student.service';
import { SignalRServiceService } from '../services/signal-rservice.service';
import { NotificationCommunicationService } from '../services/notification-communication.service';

// declare var M: any;

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit, AfterViewChecked {

  constructor(private auth: AuthService, private studentService: StudentService,
    private notify: NotificationCommunicationService) { }
  currentStudent: Student;

  isUnread: boolean;

  ngOnInit() {
    // const elem = document.querySelector('.sidenav');
    // const instance = M.Sidenav.init(elem, options);
    this.studentService.currentStudent.subscribe(data => this.currentStudent = data);
    this.notify.isUnread.subscribe(data => this.isUnread = data);
  }

  ngAfterViewChecked() { // Respond after Angular initializes the component's views and child views.
  // setTimeout( function() {

  // }, 0);
  // update de fields when the document and the views a are ready
                                   // in case that the inputs are empty
  }

  loggedOut() {
    this.auth.changeLoginStatus(false);
    this.auth.logoutUser();
  }

}
