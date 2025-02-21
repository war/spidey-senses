import { Component } from '@angular/core';
import { SpiderService } from '../../services/spider.service';

@Component({
  selector: 'app-spider',
  standalone: true,
  template: `
    <div class="absolute inset-0 flex items-center justify-center">
      <div class="relative w-6 h-6">
        <!-- Spider body -->
        <div class="absolute inset-0 bg-blue-600 rounded-full"></div>
        
        <!-- Direction indicator -->
        <div 
          class="absolute inset-0 flex items-center justify-center"
          [style.transform]="getRotation()"
        >
          <div class="w-0 h-0 border-l-4 border-r-4 border-b-8 border-l-transparent border-r-transparent border-b-white"></div>
        </div>
      </div>
    </div>
  `
})

export class SpiderComponent {
  orientation = 'Up';

  constructor(private spiderService: SpiderService) {
    this.spiderService.currentFormData.subscribe(data => {
      this.orientation = data.Orientation;
    });
  }

  getRotation(): string {
    const rotations = {
      'Up': 'rotate(0deg)',
      'Right': 'rotate(90deg)',
      'Down': 'rotate(180deg)',
      'Left': 'rotate(-90deg)'
    };

    return rotations[this.orientation as keyof typeof rotations] || 'rotate(0deg)';
  }
}
