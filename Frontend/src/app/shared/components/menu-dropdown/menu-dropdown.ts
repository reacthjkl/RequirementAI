import { Component, ElementRef, HostListener, Input } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faChevronDown, faChevronUp } from '@fortawesome/free-solid-svg-icons';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-menu-dropdown',
  imports: [FontAwesomeModule, NgbTooltip],
  templateUrl: './menu-dropdown.html',
  styleUrl: './menu-dropdown.scss',
})
export class MenuDropdown {
  @Input({ required: true }) public title!: string;
  @Input() public subtitle?: string;
  @Input() public icon?: IconDefinition;
  @Input() public iconText?: string;
  @Input() public dropup = false;
  @Input() public iconOnly = false;
  @Input() public tooltip?: string;

  public isOpen = false;
  public readonly faChevronDown = faChevronDown;
  public readonly faChevronUp = faChevronUp;

  constructor(private readonly elementRef: ElementRef<HTMLElement>) {}

  public toggle(): void {
    this.isOpen = !this.isOpen;
  }

  public close(): void {
    this.isOpen = false;
  }

  @HostListener('document:click', ['$event'])
  public closeOnOutsideClick(event: MouseEvent): void {
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
