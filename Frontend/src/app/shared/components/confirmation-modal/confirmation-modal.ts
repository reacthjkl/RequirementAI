import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-confirmation-modal',
  imports: [],
  templateUrl: './confirmation-modal.html',
})
export class ConfirmationModal {
  @Input() public title = 'Confirm action';
  @Input() public message = 'Are you sure you want to continue?';
  @Input() public cancelText = 'Cancel';
  @Input() public confirmText = 'Confirm';
  @Input() public confirmButtonClass = 'btn-danger';

  constructor(public readonly activeModal: NgbActiveModal) {}
}
