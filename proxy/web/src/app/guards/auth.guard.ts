import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

    constructor(private router: Router, private auth: AuthService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (localStorage.getItem('token') && this.auth.tokenValid) {
            // logged in so return true
            return true;
        }
        console.log('in Guard');

        // not logged in so redirect to login page with the return url
        this.router.navigate(['/login']);
        return false;
    }
}
