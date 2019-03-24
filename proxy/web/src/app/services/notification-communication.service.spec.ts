import { TestBed } from '@angular/core/testing';

import { NotificationCommunicationService } from './notification-communication.service';

describe('NotificationCommunicationService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: NotificationCommunicationService = TestBed.get(NotificationCommunicationService);
    expect(service).toBeTruthy();
  });
});
