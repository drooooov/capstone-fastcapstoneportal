import { TestBed, inject } from '@angular/core/testing';

import { SpecificGroupService } from './specific-group.service';

describe('SpecificGroupService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SpecificGroupService]
    });
  });

  it('should be created', inject([SpecificGroupService], (service: SpecificGroupService) => {
    expect(service).toBeTruthy();
  }));
});
