import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupLookingComponent } from './group-looking.component';

describe('GroupLookingComponent', () => {
  let component: GroupLookingComponent;
  let fixture: ComponentFixture<GroupLookingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupLookingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupLookingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
