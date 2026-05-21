export type StepNavigationResult = 'stay' | 'next-main-step' | 'handled-internally';

export interface ProjectWizardStep {
  canGoNext(): Promise<StepNavigationResult>;
}

