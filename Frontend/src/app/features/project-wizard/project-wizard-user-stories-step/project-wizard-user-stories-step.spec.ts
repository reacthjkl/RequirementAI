import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectWizardUserStoriesStep } from './project-wizard-user-stories-step';

describe('ProjectWizardUserStoriesStep', () => {
  let component: ProjectWizardUserStoriesStep;
  let fixture: ComponentFixture<ProjectWizardUserStoriesStep>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectWizardUserStoriesStep]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectWizardUserStoriesStep);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
