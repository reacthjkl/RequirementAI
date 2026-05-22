import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { CurrentUserService } from '../../core/services/current-user.service';
import { AuthService } from '../../shared/services/auth';
import { ProjectService } from '../../shared/services/project';

import { Menu } from './menu';

describe('Menu', () => {
  let component: Menu;
  let fixture: ComponentFixture<Menu>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Menu],
      providers: [
        provideRouter([]),
        {
          provide: CurrentUserService,
          useValue: {
            get: async () => null,
          },
        },
        {
          provide: ProjectService,
          useValue: {
            get: async () => [],
          },
        },
        {
          provide: AuthService,
          useValue: {
            logout: async () => null,
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(Menu);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
