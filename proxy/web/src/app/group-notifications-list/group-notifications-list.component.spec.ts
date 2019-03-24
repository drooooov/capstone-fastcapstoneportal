import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupNotificationsListComponent } from './group-notifications-list.component';

describe('GroupNotificationsListComponent', () => {
  let component: GroupNotificationsListComponent;
  let fixture: ComponentFixture<GroupNotificationsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupNotificationsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupNotificationsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
