import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Player } from '../models/player.model';
import { Result } from '../models/result.model';
import { MatInput } from '@angular/material/input';
import { MatFormField, MatError } from '@angular/material/form-field';

@Component({
    selector: 'memory-game-high-score-input',
    templateUrl: './high-score-input.component.html',
    styleUrls: ['./high-score-input.component.scss'],
    standalone: true,
    imports: [FormsModule, ReactiveFormsModule, MatFormField, MatInput, MatError]
})
export class HighScoreInputComponent implements OnInit, OnDestroy {
  @Input() score: number;
  @Output() onStored = new EventEmitter<any[]>();

  highScoreForm: UntypedFormGroup;

  constructor(private formBuilder: UntypedFormBuilder,) {
  }

  ngOnInit(): void {
    this.highScoreForm = this.formBuilder.group({
      name : ['', Validators.required],
    });
  }

  ngOnDestroy() {

  }

  /**
  * Save the player result in the localstorage and emit this one.
  * @param result 
  */
  private saveHOFEntry(result: Result) {
    const HOF_KEY = '::Memory::ResultsGame';
    const HOF_MAX_SIZE = 10;
    const results = JSON.parse(localStorage.getItem(HOF_KEY) || '[]');
    const insertionPoint = results.findIndex(guesses => guesses >= result.score);

    if (insertionPoint === -1)
      results.push(result);
    else
      results.splice(insertionPoint, 0, result);

    if (results.length > HOF_MAX_SIZE)
      results.splice(HOF_MAX_SIZE, results.length);

    localStorage.setItem(HOF_KEY, JSON.stringify(results));

    this.onStored.emit(results)
  }


  persistWinner() {
    const winner = this.highScoreForm.get('name').value;
    const newDate = new Date();
    const newPlayer: Player = { name: winner};
    const newResult: Result = { id: newDate, score: this.score, player: newPlayer, date: newDate };

    this.saveHOFEntry(newResult);

    this.highScoreForm.reset();
  }
}
