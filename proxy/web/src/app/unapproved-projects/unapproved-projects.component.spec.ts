import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UnapprovedProjectsComponent } from './unapproved-projects.component';

describe('UnapprovedProjectsComponent', () => {
  let component: UnapprovedProjectsComponent;
  let fixture: ComponentFixture<UnapprovedProjectsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UnapprovedProjectsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UnapprovedProjectsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
