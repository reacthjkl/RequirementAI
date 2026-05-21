import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';

import { OrganizationService } from './organization';

describe('OrganizationService', () => {
  let service: OrganizationService;

  beforeEach(() => {
    TestBed.configureTestingModule({ providers: [provideHttpClient()] });
    service = TestBed.inject(OrganizationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
