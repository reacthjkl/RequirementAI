import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectWizardPersonasStep } from './project-wizard-personas-step';

describe('ProjectWizardPersonasStep', () => {
  let component: ProjectWizardPersonasStep;
  let fixture: ComponentFixture<ProjectWizardPersonasStep>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectWizardPersonasStep]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectWizardPersonasStep);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
