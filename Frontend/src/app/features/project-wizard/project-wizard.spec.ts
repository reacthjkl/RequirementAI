import { ComponentFixture, TestBed } from '@angular/core/testing';

import { provideHttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { ProjectWizard } from './project-wizard';

describe('ProjectWizard', () => {
  let component: ProjectWizard;
  let fixture: ComponentFixture<ProjectWizard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectWizard],
      providers: [
        provideHttpClient(),
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              paramMap: {
                get: () => null,
              },
            },
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ProjectWizard);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
