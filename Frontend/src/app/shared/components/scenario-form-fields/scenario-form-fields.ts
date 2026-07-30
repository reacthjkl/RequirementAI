import { Component, Input } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';

export type ScenarioFormGroup = FormGroup<{
  id: FormControl<string>;
  title: FormControl<string>;
  content: FormControl<string>;
}>;

export type ScenarioFormControlName = keyof ScenarioFormGroup['controls'];

@Component({
  selector: 'app-scenario-form-fields',
  imports: [ReactiveFormsModule],
  templateUrl: './scenario-form-fields.html',
  styleUrl: './scenario-form-fields.scss',
})
export class ScenarioFormFields {
  @Input({ required: true }) public form!: ScenarioFormGroup;
  @Input() public idPrefix = 'scenario';

  public isInvalid(controlName: ScenarioFormControlName): boolean {
    const control = this.form.controls[controlName];
    return control.touched && control.invalid;
  }
}
