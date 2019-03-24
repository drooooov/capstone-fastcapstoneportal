import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RankProjectsComponent } from './rank-projects.component';

describe('RankProjectsComponent', () => {
  let component: RankProjectsComponent;
  let fixture: ComponentFixture<RankProjectsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RankProjectsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RankProjectsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
