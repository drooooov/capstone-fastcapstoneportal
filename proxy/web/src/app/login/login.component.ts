import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router, ActivatedRoute } from '@angular/router';
import { StudentService } from '../services/student.service';
import { GroupService } from '../services/group.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  show = true;
  returnUrl: string;
  loginList: any;
  loginUserData = {
    'username': '',
    'password': '',
    'id': ''
  };

  constructor(private _auth: AuthService, private studentService: StudentService,
    private groupService: GroupService,
    private route: ActivatedRoute,
    private _router: Router) { }

    ngOnInit() {
      // let bool;
      // this._auth.isLoggedIn.subscribe(data => bool = data);
      // if (bool) {
      //   this._router.navigate(['/home']);
      // } else {
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/home';
      // }

    }



    loginUser() {
      console.log('working?');

      this._auth.login({
        'username': this.loginUserData.username,
        'password': this.loginUserData.password
      })
      .subscribe(
        data => {
          console.log(data);
          this._router.navigate([this.returnUrl]);

          this._auth.handleAuth(data);

          if (JSON.parse(localStorage.isAdmin) === false) {
            this._auth.logInStudent();
            }
          },
          (err) => {
            console.log(err);
          });
        }
      }
