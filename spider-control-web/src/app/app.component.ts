import { Component } from '@angular/core';
import { SpiderComponent } from './components/spider/spider.component';
import { ControlsComponent } from "./components/controls/controls.component";
import { WallComponent } from "./components/wall/wall.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [SpiderComponent, ControlsComponent, WallComponent],
  template: `
    <nav class="bg-gray-800">
      <div class="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
        <div class="flex h-16 items-center justify-between">
          <div class="flex items-center">
            <div class="flex-shrink-0">
              <h1 class="text-white">Spider Control</h1>
            </div>
          </div>
        </div>
      </div>
    </nav>

    <main>
      <div class="mx-auto max-w-7xl py-6 sm:px-6 lg:px-8">
        <app-spider></app-spider>
        <app-wall></app-wall>
        <app-controls></app-controls>
      </div>
    </main>
  `
})

export class AppComponent {
  title = 'spider-control-web';
}