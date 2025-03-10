import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { ValidationMessageComponent } from './validation-message.component';
import { SpiderService } from '../../services/spider.service';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-controls',
  standalone: true,
  imports: [ReactiveFormsModule, ValidationMessageComponent],
  template: `
    <div class="flex items-center justify-center w-full h-full mx-auto p-4">
      <div class="bg-white rounded-lg shadow-lg border-2 border-gray-200">
        <div class="p-6">
          <h2 class="text-2xl font-bold mb-6">Spider Control Interface</h2>
          
          <!-- Spider Control Interface form -->
          <form [formGroup]="spiderForm" (ngSubmit)="onSubmit()" class="space-y-6">
            <!-- Wall Dimensions -->
            <div class="grid grid-cols-2 gap-4">
              <div class="space-y-2">
                <label class="block text-sm font-medium">Wall Width</label>
                <input
                  type="number"
                  formControlName="WallWidth"
                  class="w-full p-2 border rounded"
                />
                <app-validation-message
                  [control]="spiderForm.get('WallWidth')"
                  [showValidation]="showValidation"
                  errorType="required"
                  message="Wall width is required"
                />
              </div>
              
              <div class="space-y-2">
                <label class="block text-sm font-medium">Wall Height</label>
                <input
                  type="number"
                  formControlName="WallHeight"
                  class="w-full p-2 border rounded"
                />
                <app-validation-message
                  [control]="spiderForm.get('WallHeight')"
                  [showValidation]="showValidation"
                  errorType="required"
                  message="Wall height is required"
                />
              </div>
            </div>

            <!-- Spider Position and Orientation -->
            <div class="grid grid-cols-3 gap-4">
              <div class="space-y-2">
                <label class="block text-sm font-medium">Spider X Position</label>
                <input
                  type="number"
                  formControlName="SpiderX"
                  class="w-full p-2 border rounded"
                />
                <app-validation-message
                  [control]="spiderForm.get('SpiderX')"
                  [showValidation]="showValidation"
                  errorType="required"
                  message="X position is required"
                />
              </div>
              
              <div class="space-y-2">
                <label class="block text-sm font-medium">Spider Y Position</label>
                <input
                  type="number"
                  formControlName="SpiderY"
                  class="w-full p-2 border rounded"
                />
                <app-validation-message
                  [control]="spiderForm.get('SpiderY')"
                  [showValidation]="showValidation"
                  errorType="required"
                  message="Y position is required"
                />
              </div>
              
              <div class="space-y-2">
                <label class="block text-sm font-medium">Orientation</label>
                <select
                  formControlName="Orientation"
                  class="w-full p-2 border rounded"
                >
                  @for (orientation of orientations; track orientation) {
                    <option [value]="orientation">{{orientation}}</option>
                  }
                </select>
              </div>
            </div>

            <!-- Commands -->
            <div class="space-y-2">
              <label class="block text-sm font-medium">Commands (L, R, F only)</label>
              <input
                type="text"
                formControlName="Commands"
                class="w-full p-2 border rounded uppercase"
                placeholder="e.g., LRFFLRFF"
              />
              <app-validation-message
                [control]="spiderForm.get('Commands')"
                [showValidation]="showValidation"
                errorType="required"
                message="Commands are required"
              />
              <app-validation-message
                [control]="spiderForm.get('Commands')"
                [showValidation]="showValidation"
                errorType="pattern"
                message="Commands can only contain L, R, and F"
              />
            </div>

            <!-- Error Message -->
            @if (errorMessage) {
              <div class="bg-red-50 text-red-800 p-4 rounded-md">
                {{ errorMessage }}
              </div>
            }

            <!-- Success Message -->
            @if (successMessage) {
              <div class="bg-green-50 text-green-800 p-4 rounded-md">
                {{ successMessage }}
              </div>
            }

            <!-- Submit Button -->
            <button
              type="submit"
              class="w-full bg-blue-600 hover:bg-blue-700 text-white p-3 rounded-md transition-colors"
            >
              Send Commands
            </button>
          </form>
        </div>
      </div>
    </div>
  `,
  styles: [`
    :host {
      display: block;
      height: 100%;
    }
  `]
})

export class ControlsComponent {
  spiderForm: FormGroup;
  orientations = ['Up', 'Right', 'Down', 'Left'];
  errorMessage = '';
  successMessage = '';
  showValidation = false;

  constructor(
    private fb: FormBuilder, 
    private spiderService: SpiderService,
    private apiService: ApiService
  ) {
    this.spiderForm = this.fb.group({
      WallWidth: ['', [Validators.required]],
      WallHeight: ['', [Validators.required]],
      SpiderX: ['', [Validators.required]],
      SpiderY: ['', [Validators.required]],
      Orientation: ['Up', Validators.required],
      Commands: ['', [Validators.required, Validators.pattern(/^[LRF]+$/i)]]
    });

    this.spiderForm.valueChanges.subscribe(() => {
      if (this.spiderForm.valid) {
        this.spiderService.updateFormData(this.spiderForm.value);
      }
    });
  }

  onSubmit() {
    // Reset messages on form submit and enable validation
    this.showValidation = true;
    this.errorMessage = '';
    this.successMessage = '';

    // Console log form data to send to the API and show success message
    if (this.spiderForm.valid) {
      console.log('Form data:', this.spiderForm.value);

      this.apiService.processSpiderCommand(this.spiderForm.value)
        .subscribe({
          next: (response) => {
            console.log('API Response:', response);
            this.successMessage = `Commands executed successfully! Final position: ${response.finalPosition}`;
            
            // Parse the final position to update the spider location / orientation
            const [x, y, orientation] = response.finalPosition.split(' ');
            const updatedFormData = {
              ...this.spiderForm.value,
              SpiderX: parseInt(x),
              SpiderY: parseInt(y),
              Orientation: orientation
            };

            this.spiderService.updateFormData(updatedFormData);
          },
          error: (error) => {
            console.error('API Error:', error);
            this.errorMessage = error.error?.detail || 'An error occurred while processing your request';
          }
        });
    }
  }
}
