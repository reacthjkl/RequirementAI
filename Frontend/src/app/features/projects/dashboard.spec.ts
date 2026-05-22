import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { PersonaService } from '../../shared/services/persona';
import { ProjectService } from '../../shared/services/project';
import { ScenarioService } from '../../shared/services/scenario';
import { UserStoryService } from '../../shared/services/user-story';

import { Projects } from './projects';

describe('Projects', () => {
  let component: Projects;
  let fixture: ComponentFixture<Projects>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Projects],
      providers: [
        provideRouter([]),
        {
          provide: ProjectService,
          useValue: {
            get: async () => [],
          },
        },
        {
          provide: PersonaService,
          useValue: {
            getByProjectId: async () => [],
          },
        },
        {
          provide: ScenarioService,
          useValue: {
            getByPersonaId: async () => [],
          },
        },
        {
          provide: UserStoryService,
          useValue: {
            getByScenarioId: async () => [],
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(Projects);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
