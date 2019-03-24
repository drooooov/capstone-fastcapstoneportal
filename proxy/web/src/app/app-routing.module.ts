import { MatchMakingComponent } from './match-making/match-making.component';
import { MembersDisplayComponent } from './members-display/members-display.component';
import { LoginComponent } from './login/login.component';
import { Routes, RouterModule } from '@angular/router';

import { SearchStudentComponent } from './search-student/search-student.component';
import { GroupLookingComponent } from './group-looking/group-looking.component';
import { HomeGroupComponent } from './home-group/home-group.component';
import { HomeComponent } from './home/home.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { NgModule } from '@angular/core';
import { RankProjectsComponent } from './rank-projects/rank-projects.component';
import { ProjectInfoComponent } from './project-info/project-info.component';
import { ViewProjectsComponent } from './view-projects/view-projects.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { EditGroupComponent } from './edit-group/edit-group.component';
import { CreateUserComponent } from './create-user/create-user.component';
import { EditProjectComponent } from './edit-project/edit-project.component';
import { CreateProjectComponent } from './create-project/create-project.component';
import { UnapprovedProjectsComponent } from './unapproved-projects/unapproved-projects.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { ViewGroupComponent } from './view-group/view-group.component';
import { BuildAnnouncementComponent } from './build-announcement/build-announcement.component';
import { AuthGuard } from './guards/auth.guard';
import { StartupComponent } from './startup/startup.component';


const appRoutes: Routes =
    [
        {
            path: 'searchStudent',
            component: SearchStudentComponent,
            canActivate: [AuthGuard]
            // , outlet: 'showHomeLinks'
        },
        {
            path: 'member-display',
            component: MembersDisplayComponent,
            canActivate: [AuthGuard]
            // , outlet: 'showHomeLinks'
        },
        {
            path: 'viewGroups',
            component: GroupLookingComponent,
            canActivate: [AuthGuard]
            // , outlet: 'showHomeLinks'
        },
        {
            path: 'homeGroup',
            component: HomeGroupComponent,
            canActivate: [AuthGuard]
        },
        {
            path: 'profile',
            component: UserProfileComponent,
            canActivate: [AuthGuard]
        },
        {
          path: 'edit-profile',
          component: EditProfileComponent,
          canActivate: [AuthGuard]
        },
        {
          path: 'edit-project',
          component: EditProjectComponent,
          canActivate: [AuthGuard]
        },
        {
          path: 'edit-group',
          component: EditGroupComponent,
          canActivate: [AuthGuard]
        },
        {
            path: 'home',
            component: HomeComponent,
            canActivate: [AuthGuard]
        },
        {
            path: 'login',
            component: LoginComponent
        },
        {
          path: 'rank',
          component: RankProjectsComponent
        },
        {
          path: 'viewProject',
          component: ProjectInfoComponent,
          canActivate: [AuthGuard]
        },
        {
          path: 'viewProjects',
          component: ViewProjectsComponent,
          canActivate: [AuthGuard]
        },
        {
          path: 'createUser',
          component: CreateUserComponent
        },
        {
          path: 'createProject',
          component: CreateProjectComponent,
          canActivate: [AuthGuard]
        },
        {
          path: 'matching',
          component: MatchMakingComponent,
          canActivate: [AuthGuard]
        },
        {
          path: 'student-projects',
          component: UnapprovedProjectsComponent,
          canActivate: [AuthGuard]
        },
        {
          path: 'notification',
          component: NotificationsComponent,
          canActivate: [AuthGuard]
        },
        {
          path: 'viewGroup',
          component: ViewGroupComponent,
          canActivate: [AuthGuard]
        },
        {
          path: 'send-announcement',
          component: BuildAnnouncementComponent,
          canActivate: [AuthGuard]
        },
        {
          path: 'start',
          component: StartupComponent
        },
        {
            path: '',
            redirectTo: '/login',
            pathMatch: 'full'
        }
    ];


@NgModule({
    imports: [RouterModule.forRoot(appRoutes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
