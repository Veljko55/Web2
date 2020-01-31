import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LineMeshComponent } from './line-mesh.component';

describe('LineMeshComponent', () => {
  let component: LineMeshComponent;
  let fixture: ComponentFixture<LineMeshComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LineMeshComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LineMeshComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
