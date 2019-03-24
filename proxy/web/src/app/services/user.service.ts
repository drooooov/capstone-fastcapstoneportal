import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class UserService {
  constructor(private http: HttpClient) {}

  creatUser(user: User): any {
    const sentUser = {
      admin: user.isAdmin,
      program: user.ProgramName,
      password: user.Password,
      firstName: user.FirstName,
      lastName: user.LastName,
      userName: user.Username,
      email: user.Email
    };
    return this.http.post('/api/Account/Register', sentUser);
  }
}
