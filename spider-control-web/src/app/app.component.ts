import { Component } from '@angular/core';
import { ControlsComponent } from "./components/controls/controls.component";
import { WallComponent } from "./components/wall/wall.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ControlsComponent, WallComponent],
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

    <main class="h-[calc(100vh-4rem)]">
      <div class="items-center justify-center w-full h-full h-full py-6 sm:px-6 lg:px-8">
        <div class="flex flex-row gap-6 items-center justify-center">
          <div class="w-2/4 h-full">
            <app-wall></app-wall>
          </div>
          <div class="w-2/4 h-full overflow-y-auto">
            <app-controls></app-controls>
          </div>
        </div>
      </div>
    </main>
  `
})

export class AppComponent {
  title = 'spider-control-web';
}