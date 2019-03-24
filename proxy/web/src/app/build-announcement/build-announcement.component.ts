import { Component, OnInit } from '@angular/core';
import { NotificationsService } from '../services/notifications.service';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-build-announcement',
  templateUrl: './build-announcement.component.html',
  styleUrls: ['./build-announcement.component.css']
})
export class BuildAnnouncementComponent implements OnInit {

  customAnnouncement = '';
  temp_announcement = '';
  constructor( private notify: NotificationsService,
    public snackBar: MatSnackBar) { }

  ngOnInit() {

  }

  createAnnouncement() {
    this.notify.createAnnouncement(this.customAnnouncement).subscribe(data => {
      console.log(data);
      this.customAnnouncement = '';
      this.openSnackBar();
    });

  }

  openSnackBar() {
    this.snackBar.open('Announcement is sent to the students', 'Close', {
      duration: 3000,
    });
  }

}
