import { signal } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { CurrentUserService } from './core/services/current-user.service';
import { App } from './app';
import { AuthService } from './shared/services/auth';
import { ProjectService } from './shared/services/project';

describe('App', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [
        provideRouter([]),
        {
          provide: AuthService,
          useValue: {
            loggedIn: signal(true),
          },
        },
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
      ],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(App);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it('should render the application shell', async () => {
    const fixture = TestBed.createComponent(App);
    await fixture.whenStable();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('app-navbar')).toBeTruthy();
  });
});
