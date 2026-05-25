import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';

export type MetaDropdownValue = string | number;

export interface MetaDropdownOption {
  value: MetaDropdownValue;
  label: string;
  colorClass?: string;
}

@Component({
  selector: 'app-meta-dropdown',
  imports: [NgbDropdownModule],
  templateUrl: './meta-dropdown.html',
})
export class MetaDropdown {
  @Input({ required: true }) public id!: string;
  @Input() public value: MetaDropdownValue | null = null;
  @Input() public options: MetaDropdownOption[] = [];
  @Input() public placeholder = 'Select';
  @Input() public disabled = false;
  @Input() public invalid = false;

  @Output() public readonly valueChange = new EventEmitter<MetaDropdownValue>();

  public get selectedOption(): MetaDropdownOption | undefined {
    return this.options.find((option) => option.value === this.value);
  }

  public selectOption(option: MetaDropdownOption): void {
    this.valueChange.emit(option.value);
  }
}
