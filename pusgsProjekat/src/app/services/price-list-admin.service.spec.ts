import { TestBed, inject } from '@angular/core/testing';

import { PriceListAdminService } from './price-list-admin.service';

describe('PriceListAdminService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PriceListAdminService]
    });
  });

  it('should be created', inject([PriceListAdminService], (service: PriceListAdminService) => {
    expect(service).toBeTruthy();
  }));
});
