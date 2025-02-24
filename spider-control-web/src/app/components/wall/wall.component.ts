import { Component, Input } from '@angular/core';
import { SpiderService } from '../../services/spider.service';
import { SpiderComponent } from '../spider/spider.component';

@Component({
  selector: 'app-wall',
  standalone: true,
  imports: [SpiderComponent],
  template: `
    <div class="flex w-full h-full">
      <div class="w-full h-full bg-white border-2 border-gray-200 rounded-lg shadow-lg relative flex justify-center items-center p-10">
        <div class="grid-container aspect-square w-full max-h-full">
          @for (row of gridRows; track row) {
            <div class="grid-row flex">
              @for (cell of row; track cell) {
                <div 
                  class="grid-cell border border-gray-200 flex-1 aspect-square relative"
                  [class.bg-blue-100]="cell.hasSpider"
                >
                  @if (cell.hasSpider) {
                    <app-spider></app-spider>
                  }
                </div>
              }
            </div>
          }
        </div>
      </div>
    </div>
  `,
  styles: [`
    :host {
      display: block;
      width: 100%;
    }
    .grid-container {
      display: flex;
      flex-direction: column;
      overflow: hidden;
    }
    .grid-row {
      flex: 1;
      min-height: 0;
    }
    .grid-cell {
      min-width: 0;
      min-height: 0;
    }
  `]
})

export class WallComponent {
  @Input() wallWidth = 0;
  @Input() wallHeight = 0;
  @Input() spiderX = 0;
  @Input() spiderY = 0;

  gridRows: Array<Array<{hasSpider: boolean}>> = [];

  constructor(private spiderService: SpiderService) {
    this.spiderService.currentFormData.subscribe(data => {
      this.wallWidth = data.WallWidth || 0;
      this.wallHeight = data.WallHeight || 0;
      this.spiderX = data.SpiderX || 0;
      this.spiderY = data.SpiderY || 0;
      this.createGrid();
    });
  }

  ngOnChanges() {
    this.createGrid();
  }

  private createGrid() {
    if (this.wallWidth <= 0 || this.wallHeight <= 0) return;
    
    this.gridRows = Array(this.wallHeight).fill(null).map((_, rowIndex) => 
      Array(this.wallWidth).fill(null).map((_, x) => ({
        hasSpider: x === this.spiderX && (this.wallHeight - 1 - rowIndex) === this.spiderY
      }))
    );
  }
}
