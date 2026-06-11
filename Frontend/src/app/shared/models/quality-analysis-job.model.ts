import { JobStatus } from '../enums/job-status.enum';

export interface QualityAnalysisJob {
  projectId: string;
  status: JobStatus;
  errorMessage: string | null;
  startedAt: string | null;
  finishedAt: string | null;
}
