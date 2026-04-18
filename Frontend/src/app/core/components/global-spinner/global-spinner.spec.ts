import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GlobalSpinner } from './global-spinner';

describe('GlobalSpinner', () => {
  let component: GlobalSpinner;
  let fixture: ComponentFixture<GlobalSpinner>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GlobalSpinner]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GlobalSpinner);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
