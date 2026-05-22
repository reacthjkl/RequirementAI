import { Component, ElementRef, HostListener, Input } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faChevronDown, faChevronUp } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-menu-dropdown',
  imports: [FontAwesomeModule],
  templateUrl: './menu-dropdown.html',
})
export class MenuDropdown {
  @Input({ required: true }) title!: string;
  @Input() subtitle?: string;
  @Input() icon?: IconDefinition;
  @Input() iconText?: string;
  @Input() dropup = false;

  isOpen = false;
  faChevronDown = faChevronDown;
  faChevronUp = faChevronUp;

  constructor(private readonly elementRef: ElementRef<HTMLElement>) {}

  toggle(): void {
    this.isOpen = !this.isOpen;
  }

  close(): void {
    this.isOpen = false;
  }

  @HostListener('document:click', ['$event'])
  closeOnOutsideClick(event: MouseEvent): void {
    if (!this.isOpen) {
      return;
    }

    const target = event.target;

    if (target instanceof Node && this.elementRef.nativeElement.contains(target)) {
      return;
    }

    this.close();
  }
}
