import { Component, Input } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCheck, faTrash, faTriangleExclamation } from '@fortawesome/free-solid-svg-icons';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

export interface UserStoryEdgeCaseModalValue {
  preconditions: string;
  triggerAction: string;
  expectedBehavior: string;
}

export type UserStoryEdgeCaseModalResult =
  | {
      action: 'save';
      value: UserStoryEdgeCaseModalValue;
    }
  | {
      action: 'delete';
    };

@Component({
  selector: 'app-user-story-edge-case-modal',
  imports: [ReactiveFormsModule, FontAwesomeModule],
  templateUrl: './user-story-edge-case-modal.html',
})
export class UserStoryEdgeCaseModal {
  @Input() public title = 'Edit edge case';
  @Input() public canDelete = false;

  public readonly form;

  public readonly faCheck = faCheck;
  public readonly faTrash = faTrash;
  public readonly faTriangleExclamation = faTriangleExclamation;

  constructor(
    public readonly activeModal: NgbActiveModal,
    private readonly fb: FormBuilder,
  ) {
    this.form = this.fb.nonNullable.group({
      preconditions: ['', Validators.required],
      triggerAction: ['', Validators.required],
      expectedBehavior: ['', Validators.required],
    });
  }

  @Input() public set value(value: UserStoryEdgeCaseModalValue | null) {
    if (!value) {
      return;
    }

    this.form.patchValue(value);
    this.form.markAsPristine();
  }

  public save(): void {
    this.form.markAllAsTouched();

    if (this.form.invalid) {
      return;
    }

    const value = this.form.getRawValue();

    this.activeModal.close({
      action: 'save',
      value: {
        preconditions: value.preconditions.trim(),
        triggerAction: value.triggerAction.trim(),
        expectedBehavior: value.expectedBehavior.trim(),
      },
    } satisfies UserStoryEdgeCaseModalResult);
  }

  public delete(): void {
    this.activeModal.close({ action: 'delete' } satisfies UserStoryEdgeCaseModalResult);
  }

  public isInvalid(controlName: keyof UserStoryEdgeCaseModalValue): boolean {
    const control = this.form.controls[controlName];
    return control.touched && control.invalid;
  }
}
