import { TestBed } from '@angular/core/testing';

import { ProjectWizardState } from './project-wizard-state';

describe('ProjectWizardState', () => {
  let service: ProjectWizardState;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProjectWizardState);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
