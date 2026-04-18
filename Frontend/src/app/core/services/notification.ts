import { Inject, Injectable } from '@angular/core';
import { Notyf } from 'notyf';
import { NOTYF } from '../configs/notyf.injection-token';

@Injectable({
  providedIn: 'root',
})
export class Notification {
  constructor(@Inject(NOTYF) private notyf: Notyf) {}

  public success(message: string) {
    this.notyf.success(message);
  }

  public fail(message: string) {
    this.notyf.error(message);
  }
}
