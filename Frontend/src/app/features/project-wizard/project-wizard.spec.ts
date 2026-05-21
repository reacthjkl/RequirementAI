import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectWizzard } from './project-wizzard';

describe('ProjectWizzard', () => {
  let component: ProjectWizzard;
  let fixture: ComponentFixture<ProjectWizzard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectWizzard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectWizzard);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
