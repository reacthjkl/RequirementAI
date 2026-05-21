import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-project-wizard-progress',
  imports: [],
  templateUrl: './project-wizard-progress.html',
  styleUrl: './project-wizard-progress.scss',
})
export class ProjectWizardProgress {
  @Input({ required: true }) title = '';
  @Input({ required: true }) currentStepIndex = 0;
  @Input({ required: true }) stepCount = 0;
  @Input({ required: true }) progress = 0;
}

