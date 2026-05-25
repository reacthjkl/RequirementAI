import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCheck } from '@fortawesome/free-solid-svg-icons';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-project-refine-modal',
  imports: [FormsModule, FontAwesomeModule],
  templateUrl: './project-refine-modal.html',
})
export class ProjectRefineModal {
  public customInstructions = '';
  public readonly faCheck = faCheck;

  constructor(public readonly activeModal: NgbActiveModal) {}

  public refine(): void {
    const instructions = this.customInstructions.trim();
    this.activeModal.close(instructions || null);
  }
}
