import { TestBed } from '@angular/core/testing';

import { SignalRServiceService } from './signal-rservice.service';

describe('SignalRServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SignalRServiceService = TestBed.get(SignalRServiceService);
    expect(service).toBeTruthy();
  });
});
