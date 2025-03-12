import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-validation-message',
  standalone: true,
  template: `
    @if (showValidation && control?.errors?.[errorType]) {
      <span class="text-red-500 text-sm">{{ message }}</span>
    }
  `
})

export class ValidationMessageComponent {
  @Input() control: any;
  @Input() showValidation: boolean = false;
  @Input() errorType: string = '';
  @Input() message: string = '';
}
