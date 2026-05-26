import { Component, Input } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCheck, faTrash } from '@fortawesome/free-solid-svg-icons';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

export interface UserStoryAcceptanceCriteriaModalValue {
  wording: string;
}

export type UserStoryAcceptanceCriteriaModalResult =
  | {
      action: 'save';
      value: UserStoryAcceptanceCriteriaModalValue;
    }
  | {
      action: 'delete';
    };

@Component({
  selector: 'app-user-story-acceptance-criteria-modal',
  imports: [ReactiveFormsModule, FontAwesomeModule],
  templateUrl: './user-story-acceptance-criteria-modal.html',
})
export class UserStoryAcceptanceCriteriaModal {
  @Input() public title = 'Edit acceptance criteria';
  @Input() public canDelete = false;

  public readonly form;

  public readonly faCheck = faCheck;
  public readonly faTrash = faTrash;

  constructor(
    public readonly activeModal: NgbActiveModal,
    private readonly fb: FormBuilder,
  ) {
    this.form = this.fb.nonNullable.group({
      wording: ['', Validators.required],
    });
  }

  @Input() public set value(value: UserStoryAcceptanceCriteriaModalValue | null) {
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
        wording: value.wording.trim(),
      },
    } satisfies UserStoryAcceptanceCriteriaModalResult);
  }

  public delete(): void {
    this.activeModal.close({ action: 'delete' } satisfies UserStoryAcceptanceCriteriaModalResult);
  }

  public isInvalid(): boolean {
    const control = this.form.controls.wording;
    return control.touched && control.invalid;
  }
}
