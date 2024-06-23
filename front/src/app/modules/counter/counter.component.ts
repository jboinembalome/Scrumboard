import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';

@Component({
    selector: 'app-counter-component',
    templateUrl: './counter.component.html',
    standalone: true,
    imports: [MatButtonModule]
})
export class CounterComponent {
  currentCount = 0;

  public incrementCounter() {
    this.currentCount++;
  }

  public decrementCounter() {
    this.currentCount--;
  }
}
