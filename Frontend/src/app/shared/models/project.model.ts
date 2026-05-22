import { ProjectStatus } from '../enums/project-status.enum';
import { RefinementStatus } from '../enums/refinement-status.enum';

export interface Project {
  id: string;
  name: string;
  description: string;
  createdAt: string;
  status: ProjectStatus;
  refinementStatus: RefinementStatus;
}
