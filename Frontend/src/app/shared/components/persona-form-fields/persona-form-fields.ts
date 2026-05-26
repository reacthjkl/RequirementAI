import { Component, Input } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';

export type PersonaFormGroup = FormGroup<{
  id: FormControl<string>;
  name: FormControl<string>;
  description: FormControl<string>;
  contextOfUse: FormControl<string>;
  goals: FormControl<string>;
  frustrations: FormControl<string>;
}>;

export type PersonaFormControlName = keyof PersonaFormGroup['controls'];

@Component({
  selector: 'app-persona-form-fields',
  imports: [ReactiveFormsModule],
  templateUrl: './persona-form-fields.html',
})
export class PersonaFormFields {
  @Input({ required: true }) public form!: PersonaFormGroup;
  @Input() public idPrefix = 'persona';

  public isInvalid(controlName: PersonaFormControlName): boolean {
    const control = this.form.controls[controlName];
    return control.touched && control.invalid;
  }
}
