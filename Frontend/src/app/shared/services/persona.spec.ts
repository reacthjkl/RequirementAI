import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';

import { PersonaService } from './persona';

describe('PersonaService', () => {
  let service: PersonaService;

  beforeEach(() => {
    TestBed.configureTestingModule({ providers: [provideHttpClient()] });
    service = TestBed.inject(PersonaService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
