import { provideHttpClient } from '@angular/common/http';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';

import { ProjectBoard } from './project-board';

describe('ProjectBoard', () => {
  let component: ProjectBoard;
  let fixture: ComponentFixture<ProjectBoard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectBoard],
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

    fixture = TestBed.createComponent(ProjectBoard);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

