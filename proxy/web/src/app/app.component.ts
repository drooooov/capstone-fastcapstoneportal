import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  constructor(private auth: AuthService, private router: Router) {}
  isLoggedIn: boolean;

  ngOnInit() {
    this.auth.isLoggedIn
    .subscribe(arg => {
      console.log(arg, 'working?');

      this.isLoggedIn = arg;
    });

  //   this.router.events.subscribe((res) => {
  //     if (this.router.url === '/start') {
  //       console.log(this.router.url, 'Current URL');
  //     }
  // });
  }
}
