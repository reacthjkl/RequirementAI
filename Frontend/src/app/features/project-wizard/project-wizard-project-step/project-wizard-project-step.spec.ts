import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectWizardProjectStep } from './project-wizard-project-step';

describe('ProjectWizardProjectStep', () => {
  let component: ProjectWizardProjectStep;
  let fixture: ComponentFixture<ProjectWizardProjectStep>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectWizardProjectStep]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectWizardProjectStep);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
