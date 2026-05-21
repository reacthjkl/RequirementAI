import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';

import { AcceptanceCriteriaService } from './acceptance-criteria';

describe('AcceptanceCriteriaService', () => {
  let service: AcceptanceCriteriaService;

  beforeEach(() => {
    TestBed.configureTestingModule({ providers: [provideHttpClient()] });
    service = TestBed.inject(AcceptanceCriteriaService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
