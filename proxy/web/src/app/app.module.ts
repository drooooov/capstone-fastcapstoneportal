import { BarRatingModule } from 'ngx-bar-rating';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule} from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { HomeComponent } from './home/home.component';
import { SearchStudentComponent } from './search-student/search-student.component';
import { GroupLookingComponent } from './group-looking/group-looking.component';
import { TitleComponent } from './title/title.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { HomeGroupComponent } from './home-group/home-group.component';
import { StudentService } from './services/student.service';
import { LoginComponent } from './login/login.component';
import { AuthService } from './services/auth.service';
import { MembersDisplayComponent } from './members-display/members-display.component';
import { GroupService } from './services/group.service';
import { SpecificGroupService } from './services/specific-group.service';
import { NotificationsService } from './services/notifications.service';
import { DashboardComponent } from './admin/dashboard/dashboard.component';
import { ProjectInfoComponent } from './project-info/project-info.component';
import { RankProjectsComponent } from './rank-projects/rank-projects.component';
import { StarsComponent } from './stars/stars.component';
import { ViewProjectsComponent } from './view-projects/view-projects.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { EditGroupComponent } from './edit-group/edit-group.component';
import { CreateUserComponent } from './create-user/create-user.component';
import { EditProjectComponent } from './edit-project/edit-project.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { NotificationsTabComponent } from './notifications-tab/notifications-tab.component';
import { NotificationsBodyComponent } from './notifications-body/notifications-body.component';
import { NavbarLoginComponent } from './navbar-login/navbar-login.component';
import { CreateProjectComponent } from './create-project/create-project.component';
import { ProjectService } from './services/project.service';
import { MatchMakingComponent } from './match-making/match-making.component';
import { UnapprovedProjectsComponent } from './unapproved-projects/unapproved-projects.component';
import { UnapprovedProjectsListComponent } from './unapproved-projects-list/unapproved-projects-list.component';
import { ComponentCommunicationService } from './services/component-communication.service';
import { ViewGroupComponent } from './view-group/view-group.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LayoutModule } from '@angular/cdk/layout';
import { MatTabsModule, MatSnackBar, MatSnackBarModule, MatChipsModule, MatFormFieldModule, MatIconModule } from '@angular/material';
import { NotificationsListComponent } from './notifications-list/notifications-list.component';
import { GroupNotificationsListComponent } from './group-notifications-list/group-notifications-list.component';
import { BuildAnnouncementComponent } from './build-announcement/build-announcement.component';
import { AuthGuard } from './guards/auth.guard';
import { LoadingScreenComponent } from './loading-screen/loading-screen.component';
import { StartupComponent } from './startup/startup.component';




@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    HomeComponent,
    SearchStudentComponent,
    GroupLookingComponent,
    TitleComponent,
    UserProfileComponent,
    HomeGroupComponent,
    LoginComponent,
    MembersDisplayComponent,
    DashboardComponent,
    ProjectInfoComponent,
    RankProjectsComponent,
    StarsComponent,
    ViewProjectsComponent,
    EditProfileComponent,
    EditGroupComponent,
    CreateUserComponent,
    EditProjectComponent,
    NotificationsComponent,
    NotificationsTabComponent,
    NotificationsBodyComponent,
    NavbarLoginComponent,
    CreateProjectComponent,
    MatchMakingComponent,
    UnapprovedProjectsComponent,
    UnapprovedProjectsListComponent,
    ViewGroupComponent,
    NotificationsListComponent,
    GroupNotificationsListComponent,
    BuildAnnouncementComponent,
    LoadingScreenComponent,
    StartupComponent
  ],
  imports: [
  BrowserModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    BarRatingModule,
    BrowserAnimationsModule,
    LayoutModule,
    MatTabsModule,
    MatSnackBarModule,
    MatChipsModule,
    MatFormFieldModule,
    MatIconModule
  ],
  providers: [StudentService, ComponentCommunicationService,
     AuthService, AuthGuard, GroupService, ProjectService,
     SpecificGroupService, NotificationsService],
  bootstrap: [AppComponent]
})
export class AppModule { }
