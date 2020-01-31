import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LineMeshAdminComponent } from './line-mesh-admin.component';

describe('LineMeshAdminComponent', () => {
  let component: LineMeshAdminComponent;
  let fixture: ComponentFixture<LineMeshAdminComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LineMeshAdminComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LineMeshAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
