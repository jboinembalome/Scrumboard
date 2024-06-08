import { Component } from '@angular/core';
import { TitleComponent } from '../../shared/components/title/title.component';

@Component({
    selector: 'app-counter-component',
    templateUrl: './counter.component.html',
    standalone: true,
    imports: [TitleComponent]
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
