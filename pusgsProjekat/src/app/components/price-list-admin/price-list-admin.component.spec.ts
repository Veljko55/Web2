import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PriceListAdminComponent } from './price-list-admin.component';

describe('PriceListAdminComponent', () => {
  let component: PriceListAdminComponent;
  let fixture: ComponentFixture<PriceListAdminComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PriceListAdminComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PriceListAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
