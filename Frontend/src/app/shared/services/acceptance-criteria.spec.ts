import { TestBed } from '@angular/core/testing';

import { AcceptanceCriteria } from './acceptance-criteria';

describe('AcceptanceCriteria', () => {
  let service: AcceptanceCriteria;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AcceptanceCriteria);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
