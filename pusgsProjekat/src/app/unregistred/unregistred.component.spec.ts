import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UnregistredComponent } from './unregistred.component';

describe('UnregistredComponent', () => {
  let component: UnregistredComponent;
  let fixture: ComponentFixture<UnregistredComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UnregistredComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UnregistredComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
