import { InjectionToken } from '@angular/core';
import { Notyf } from 'notyf';

export const NOTYF = new InjectionToken<Notyf>('NotyfToken');

// Set global Notyf configuration here
export function notyfFactory(): Notyf {
  return new Notyf({
    duration: 5000,
    position: { x: 'right', y: 'bottom' },
    dismissible: true,
  });
}
