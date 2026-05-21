import { TestBed } from '@angular/core/testing';

import { NOTYF } from '../configs/notyf.injection-token';
import { Notification } from './notification.service';

describe('Notification', () => {
  let service: Notification;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: NOTYF,
          useValue: {
            success: () => undefined,
            error: () => undefined,
          },
        },
      ],
    });
    service = TestBed.inject(Notification);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
