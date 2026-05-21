import { TestBed } from '@angular/core/testing';

import { UserStory } from './user-story';

describe('UserStory', () => {
  let service: UserStory;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserStory);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
