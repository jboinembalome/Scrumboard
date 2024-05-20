import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Result } from '../models/result.model';

@Component({
  selector: 'memory-game-results',
  templateUrl: './results.component.html',
  styleUrls: ['./results.component.scss']
})
export class ResultsComponent implements OnInit, OnDestroy {
  @Input() results: Result[];

  columns: string[] = ['Date', 'Score', 'Player'];

  constructor() {
  }

  ngOnInit(): void {
    
  }

  ngOnDestroy() {
    
  }
}
