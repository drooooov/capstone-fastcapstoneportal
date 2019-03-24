import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BuildAnnouncementComponent } from './build-announcement.component';

describe('BuildAnnouncementComponent', () => {
  let component: BuildAnnouncementComponent;
  let fixture: ComponentFixture<BuildAnnouncementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BuildAnnouncementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BuildAnnouncementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
