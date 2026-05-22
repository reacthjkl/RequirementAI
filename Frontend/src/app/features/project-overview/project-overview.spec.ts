import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { type Mock, vi } from 'vitest';
import { Notification } from '../../core/services/notification.service';
import { ProjectStatus } from '../../shared/enums/project-status.enum';
import { RefinementStatus } from '../../shared/enums/refinement-status.enum';
import { UserStoryStage } from '../../shared/enums/user-story-stage.enum';
import { Project } from '../../shared/models/project.model';
import { PersonaService } from '../../shared/services/persona';
import { ProjectService } from '../../shared/services/project';
import { ScenarioService } from '../../shared/services/scenario';
import { UserStoryService } from '../../shared/services/user-story';
import { ProjectOverview } from './project-overview';

describe('ProjectOverview', () => {
  let component: ProjectOverview;
  let fixture: ComponentFixture<ProjectOverview>;
  let projectService: { getById: Mock; update: Mock };
  let notification: { success: Mock; fail: Mock };

  const project: Project = {
    id: 'project-1',
    name: 'Checkout',
    description: 'Improve checkout flows',
    createdAt: '2026-05-22T00:00:00Z',
    status: ProjectStatus.Complete,
    refinementStatus: RefinementStatus.Completed,
  };

  beforeEach(async () => {
    projectService = {
      getById: vi.fn().mockResolvedValue(project),
      update: vi.fn().mockResolvedValue(undefined),
    };
    notification = {
      success: vi.fn(),
      fail: vi.fn(),
    };

    await TestBed.configureTestingModule({
      imports: [ProjectOverview],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              paramMap: {
                get: () => 'project-1',
              },
            },
          },
        },
        {
          provide: ProjectService,
          useValue: projectService,
        },
        {
          provide: PersonaService,
          useValue: {
            getByProjectId: async () => [{ id: 'persona-1' }, { id: 'persona-2' }],
          },
        },
        {
          provide: ScenarioService,
          useValue: {
            getByPersonaId: async (personaId: string) =>
              personaId === 'persona-1' ? [{ id: 'scenario-1' }, { id: 'scenario-2' }] : [],
          },
        },
        {
          provide: UserStoryService,
          useValue: {
            getByProjectId: async () => [
              { id: 'story-1', stage: UserStoryStage.Closed },
              { id: 'story-2', stage: UserStoryStage.Active },
            ],
          },
        },
        {
          provide: Notification,
          useValue: notification,
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ProjectOverview);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('loads project stats and progress', () => {
    expect(component.project).toEqual(project);
    expect(component.personaCount).toBe(2);
    expect(component.scenarioCount).toBe(2);
    expect(component.userStoryCount).toBe(2);
    expect(component.closedUserStoryCount).toBe(1);
    expect(component.progressPercent).toBe(50);
  });

  it('updates project details', async () => {
    component.startEditing();
    component.form.setValue({
      name: 'Checkout v2',
      description: 'Updated description',
    });

    await component.saveProject();

    expect(projectService.update).toHaveBeenCalledWith({
      id: 'project-1',
      name: 'Checkout v2',
      description: 'Updated description',
    });
    expect(component.project?.name).toBe('Checkout v2');
    expect(component.editing).toBe(false);
    expect(notification.success).toHaveBeenCalledWith('Project updated');
  });
});
