import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';

import { EdgeCaseService } from './edge-case';

describe('EdgeCaseService', () => {
  let service: EdgeCaseService;

  beforeEach(() => {
    TestBed.configureTestingModule({ providers: [provideHttpClient()] });
    service = TestBed.inject(EdgeCaseService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
