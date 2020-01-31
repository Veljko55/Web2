import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidateProfileComponent } from './validate-profile.component';

describe('ValidateProfileComponent', () => {
  let component: ValidateProfileComponent;
  let fixture: ComponentFixture<ValidateProfileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidateProfileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidateProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
