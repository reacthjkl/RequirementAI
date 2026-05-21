import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';

import { ScenarioService } from './scenario';

describe('ScenarioService', () => {
  let service: ScenarioService;

  beforeEach(() => {
    TestBed.configureTestingModule({ providers: [provideHttpClient()] });
    service = TestBed.inject(ScenarioService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
