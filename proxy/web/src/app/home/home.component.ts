import { Component, OnInit } from '@angular/core';
import { SignalRServiceService } from '../services/signal-rservice.service';
import { AuthService } from '../services/auth.service';
import { NotificationsService } from '../services/notifications.service';
import { Notifications } from '../models/notifications.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private sigService: SignalRServiceService, private auth: AuthService,
    private entreaty: NotificationsService) { }

  isAdmin: string;
  announcements: Notifications[] = [];
    ngOnInit() {
    this.sigService.connect();
    this.isAdmin = localStorage.getItem('isAdmin');
    console.log(this.isAdmin);
     this.entreaty.getAllAnnouncements().subscribe(data => {
      this.announcements = data;
      console.log(data);
    });
  }

}
