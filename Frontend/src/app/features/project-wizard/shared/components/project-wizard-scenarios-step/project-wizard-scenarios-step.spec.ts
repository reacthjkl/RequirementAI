import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectWizardScenariosStep } from './project-wizard-scenarios-step';

describe('ProjectWizardScenariosStep', () => {
  let component: ProjectWizardScenariosStep;
  let fixture: ComponentFixture<ProjectWizardScenariosStep>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectWizardScenariosStep]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectWizardScenariosStep);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
