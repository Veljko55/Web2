import { TestBed, inject } from '@angular/core/testing';

import { LineMeshAdminService } from './line-mesh-admin.service';

describe('LineMeshAdminService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LineMeshAdminService]
    });
  });

  it('should be created', inject([LineMeshAdminService], (service: LineMeshAdminService) => {
    expect(service).toBeTruthy();
  }));
});
