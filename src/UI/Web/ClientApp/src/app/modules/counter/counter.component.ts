import { Component } from '@angular/core';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html'
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
