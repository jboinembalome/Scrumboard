import { Component, Input, OnDestroy, OnInit } from '@angular/core';

@Component({
    selector: 'memory-game-score',
    templateUrl: './score.component.html',
    styleUrls: ['./score.component.scss'],
    standalone: true
})
export class ScoreComponent implements OnInit, OnDestroy {
  @Input() score: number;
  
  constructor() {
  }

  ngOnInit(): void {
    
  }

  ngOnDestroy() {
    
  }
}
