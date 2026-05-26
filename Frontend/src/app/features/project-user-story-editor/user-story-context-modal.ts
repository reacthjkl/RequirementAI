import { Component, Input } from '@angular/core';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCircleInfo } from '@fortawesome/free-solid-svg-icons';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

export interface UserStoryContextModalField {
  label: string;
  value: string;
}

@Component({
  selector: 'app-user-story-context-modal',
  imports: [FontAwesomeModule],
  templateUrl: './user-story-context-modal.html',
})
export class UserStoryContextModal {
  @Input() public title = 'Details';
  @Input() public icon: IconDefinition = faCircleInfo;
  @Input() public fields: UserStoryContextModalField[] = [];

  constructor(public readonly activeModal: NgbActiveModal) {}
}
