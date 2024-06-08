import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { Card } from '../models/card.model';
import { NgClass } from '@angular/common';

@Component({
    selector: 'memory-game-card',
    templateUrl: './card.component.html',
    styleUrls: ['./card.component.scss'],
    standalone: true,
    imports: [NgClass]
})
export class CardComponent implements OnInit, OnDestroy {
  @Input() card: Card;
  @Output() cardClick = new EventEmitter<number>();

  MASKED_SYMBOL = '❓';


  constructor() {
  }

  ngOnInit(): void {
    
  }

  ngOnDestroy() {
    
  }
}
