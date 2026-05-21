import { ProjectStatus } from '../enums/project-status.enum';

export interface Project {
  id: string;
  name: string;
  description: string;
  createdAt: string;
  status: ProjectStatus;
}
