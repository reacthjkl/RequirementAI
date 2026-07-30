import { Component, Input } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';

export type UserStoryFormGroup = FormGroup<{
  id: FormControl<string>;
  title: FormControl<string>;
  description: FormControl<string>;
}>;

export type UserStoryFormControlName = keyof UserStoryFormGroup['controls'];

@Component({
  selector: 'app-user-story-form-fields',
  imports: [ReactiveFormsModule],
  templateUrl: './user-story-form-fields.html',
})
export class UserStoryFormFields {
  @Input({ required: true }) public form!: UserStoryFormGroup;
  @Input() public idPrefix = 'userStory';

  public isInvalid(controlName: UserStoryFormControlName): boolean {
    const control = this.form.controls[controlName];
    return control.touched && control.invalid;
  }
}
