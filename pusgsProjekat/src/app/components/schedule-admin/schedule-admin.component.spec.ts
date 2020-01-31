import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleAdminComponent } from './schedule-admin.component';

describe('ScheduleAdminComponent', () => {
  let component: ScheduleAdminComponent;
  let fixture: ComponentFixture<ScheduleAdminComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScheduleAdminComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
