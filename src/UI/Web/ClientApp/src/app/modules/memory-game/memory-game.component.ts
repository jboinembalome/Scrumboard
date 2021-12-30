import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { shuffle } from 'lodash-es';
import { CountdownComponent, CountdownConfig } from 'ngx-countdown';
import { Card } from './models/card.model';
import { Result } from './models/result.model';

import { Feedback } from './models/feedback.enum';

@Component({
  selector: 'app-memory-game',
  templateUrl: './memory-game.component.html',
  styleUrls: ['./memory-game.component.scss']
})

export class MemoryGameComponent implements OnInit {
  private SYMBOLS = ["😀", "🎉", "💖", "🎩", "🐶", "🐱", "🦄", "🐬", "🌍", "🌛", "🌞", "💫", "🍎", "🍌", "🍓", "🍐", "🍟", "🍿"];
  private VISUAL_PAUSE_MSECS = 750;

  cards: Card[] = null;
  currentPair: any[] = [];
  score: number = 0;
  results: Result[] = null;
  matchedCardIds: any[] = [];
  won: boolean;
  disablePlay:boolean;
  displayTimer: boolean;
  config: CountdownConfig = {
    leftTime: 5,
    demand: true,
    formatDate: ({ date }) => `${date / 1000}`,
  };

  @ViewChild('countdown') counter: CountdownComponent;

  constructor(private _changeDetectorRef: ChangeDetectorRef) {
  }

  ngOnInit(): void {
  }

  displayResults(results: Result[]) {
    this.results = results;
  }

  generateCards() {
    const symbols = [];
    const size = 6 * 5;
    const candidates = shuffle(this.SYMBOLS);

    while (symbols.length < size) {
      const symbol = candidates.pop();

      symbols.push(symbol, symbol);
    }

    return shuffle(symbols).map((symbol, index) => <Card>{
      index: index,
      symbol: symbol,
      feedback: Feedback.masked
    });
  }

  playGame(): void {
    this.cards = this.generateCards();

    this.diplayAllCards();

    this.disablePlay = true;
    this.displayTimer = true;
    this._changeDetectorRef.detectChanges();

    this.counter.begin();
  }

  onTimerFinished(event: Event) {
    if (event["action"] == "done") {
      this.hideAllCards();
      this.displayTimer = false;
    }
  }

  diplayAllCards() {
    this.cards.forEach(c => c.feedback = Feedback.visible);
  }

  hideAllCards() {
    this.cards.forEach(c => c.feedback = Feedback.masked);
  }

  getFeedbackForCard(index: number) {
    const indexMatched = this.matchedCardIds.includes(index);

    if (this.currentPair.length < 2)
      return indexMatched || index === this.currentPair[0] ? Feedback.visible : Feedback.masked;

    if (this.currentPair.includes(index))
      return indexMatched ? Feedback.justMatched : Feedback.justMismatched;

    return indexMatched ? Feedback.visible : Feedback.masked;
  }

  handleCardClick(index: number) {
    if (this.currentPair.length === 2)
      return;

    if (this.currentPair.length === 0) {
      this.currentPair = [index];
      this.cards[index].feedback = this.getFeedbackForCard(index);
      return;
    }

    this.handleNewPairClosedBy(index);
  }

  handleNewPairClosedBy(index: number) {
    const newPair = [this.currentPair[0], index];
    const updatedScore = this.score + 1;
    const matched = this.cards[newPair[0]].symbol === this.cards[newPair[1]].symbol;

    this.currentPair = newPair;
    this.score = updatedScore;

    if (matched) {
      this.matchedCardIds = [...this.matchedCardIds, ...newPair];
      this.cards[newPair[0]].feedback = this.getFeedbackForCard(newPair[0]);
      this.cards[newPair[1]].feedback = this.getFeedbackForCard(newPair[1]);
      this.currentPair = [];
      this.won = this.matchedCardIds.length === this.cards.length;

      if(this.won)
        this.disablePlay = false;
      return;
    }

    this.cards[newPair[0]].feedback = this.getFeedbackForCard(newPair[0]);
    this.cards[newPair[1]].feedback = this.getFeedbackForCard(newPair[1]);

    setTimeout(() => {
      this.currentPair = [];
      this.cards[newPair[0]].feedback = this.getFeedbackForCard(newPair[0]);
      this.cards[newPair[1]].feedback = this.getFeedbackForCard(newPair[1]);
    }, this.VISUAL_PAUSE_MSECS);
  }

  /**
  * Tracks by function for ngFor loops.
  *
  * @param index
  * @param item
  */
  trackByFn(index: number, item: any): any {
    return item.id || index;
  }
}
