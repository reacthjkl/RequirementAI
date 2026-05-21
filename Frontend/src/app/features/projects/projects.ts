import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { Project } from '../../shared/models/project.model';
import { ProjectService } from '../../shared/services/project';

@Component({
  selector: 'app-projects',
  imports: [FontAwesomeModule, RouterModule],
  templateUrl: './projects.html',
  styleUrl: './projects.scss',
})
export class Projects {
  public projects: Project[] = [];

  //icons
  faPlus = faPlus;

  constructor(private projectSvc: ProjectService) {}

  async ngOnInit() {
    this.projects = await this.projectSvc.get();
  }
}
