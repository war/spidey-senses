import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-controls',
  standalone: true,
  imports: [ReactiveFormsModule],
  template: `
    <div class="w-full max-w-2xl mx-auto p-6">
      <div class="bg-white rounded-lg shadow-lg">
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
                  min="1"
                />
              </div>
              
              <div class="space-y-2">
                <label class="block text-sm font-medium">Wall Height</label>
                <input
                  type="number"
                  formControlName="WallHeight"
                  class="w-full p-2 border rounded"
                  min="1"
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
                  min="0"
                />
              </div>
              
              <div class="space-y-2">
                <label class="block text-sm font-medium">Spider Y Position</label>
                <input
                  type="number"
                  formControlName="SpiderY"
                  class="w-full p-2 border rounded"
                  min="0"
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
  `
})

export class ControlsComponent {
  spiderForm: FormGroup;
  orientations = ['Up', 'Right', 'Down', 'Left'];
  errorMessage = '';
  successMessage = '';

  constructor(private fb: FormBuilder) {
    this.spiderForm = this.fb.group({
      WallWidth: [''],
      WallHeight: [''],
      SpiderX: [''],
      SpiderY: [''],
      Orientation: ['Up'],
      Commands: ['']
    });
  }

  onSubmit() {
    // Reset messages on form submit
    this.errorMessage = '';
    this.successMessage = '';

    // Console log form data to send to the API
    console.log('Form data:', this.spiderForm.value);
    
    this.successMessage = 'Commands submitted successfully!';
  }
}