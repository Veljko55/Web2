import { TestBed, inject } from '@angular/core/testing';

import { ScheduleAdminService } from './schedule-admin.service';

describe('ScheduleAdminService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ScheduleAdminService]
    });
  });

  it('should be created', inject([ScheduleAdminService], (service: ScheduleAdminService) => {
    expect(service).toBeTruthy();
  }));
});
