import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UnapprovedProjectsListComponent } from './unapproved-projects-list.component';

describe('UnapprovedProjectsListComponent', () => {
  let component: UnapprovedProjectsListComponent;
  let fixture: ComponentFixture<UnapprovedProjectsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UnapprovedProjectsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UnapprovedProjectsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
