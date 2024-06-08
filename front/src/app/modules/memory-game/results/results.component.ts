import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Result } from '../models/result.model';
import { NgClass, DatePipe } from '@angular/common';

@Component({
    selector: 'memory-game-results',
    templateUrl: './results.component.html',
    styleUrls: ['./results.component.scss'],
    standalone: true,
    imports: [NgClass, DatePipe]
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
