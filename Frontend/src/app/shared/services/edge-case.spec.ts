import { TestBed } from '@angular/core/testing';

import { EdgeCase } from './edge-case';

describe('EdgeCase', () => {
  let service: EdgeCase;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EdgeCase);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
