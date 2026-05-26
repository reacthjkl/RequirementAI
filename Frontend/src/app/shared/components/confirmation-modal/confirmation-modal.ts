import { Component, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-confirmation-modal',
  imports: [FormsModule],
  templateUrl: './confirmation-modal.html',
})
export class ConfirmationModal {
  @Input() public title = 'Confirm action';
  @Input() public message = 'Are you sure you want to continue?';
  @Input() public cancelText = 'Cancel';
  @Input() public confirmText = 'Confirm';
  @Input() public confirmButtonClass = 'btn-danger';
  @Input() public requiredConfirmationText: string | null = null;
  @Input() public confirmationLabel = 'Type the name to confirm';

  public confirmationText = '';

  constructor(public readonly activeModal: NgbActiveModal) {}

  public get canConfirm(): boolean {
    return (
      this.requiredConfirmationText === null ||
      this.confirmationText === this.requiredConfirmationText
    );
  }
}
