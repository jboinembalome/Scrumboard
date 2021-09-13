import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { tap, debounceTime, takeUntil } from 'rxjs/operators';
import { LabelDto } from 'src/app/swagger';


@Component({
  selector: 'scrumboard-dialog-card',
  templateUrl: './dialog-card.component.html',
  styleUrls: ['./dialog-card.component.scss']
})
export class DialogCardComponent implements OnInit, OnDestroy {

  private _unsubscribeAll: Subject<any> = new Subject<any>();
  //board: Board;
  card: any; // Replace by CardDetail
  cardForm: FormGroup;
  labels: LabelDto[];
  filteredLabels: LabelDto[];

  constructor(
    public matDialogRef: MatDialogRef<DialogCardComponent>,
    private _formBuilder: FormBuilder) {
  }

  ngOnInit(): void {
/*         // Get the board
        this._scrumboardService.board$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((board) => {

                // Board data
                this.board = board;

                // Get the labels
                this.labels = this.filteredLabels = board.labels;
            });
 */

/*         // Get the card details
        this._scrumboardService.card$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((card) => {
                this.card = card;
            });
 */
        // Prepare the card form
        this.cardForm = this._formBuilder.group({
            id : [''],
            name : ['', Validators.required],
            description : [''],
            labels : [[]],
            dueDate : [null]
        });

        // Fill the form
        this.cardForm.setValue({
            id : this.card.id,
            name : this.card.name,
            description : this.card.description,
            labels : this.card.labels,
            dueDate : this.card.dueDate
        });

        // Update card when there is a value change on the card form
        this.cardForm.valueChanges
            .pipe(
                tap((value) => {

                    // Update the card object (Maybe use lodash? like below)
                    //this.card = assign(this.card, value);
                    this.card.id = value.id;
                    this.card.title = value.name;
                    this.card.description = value.description;
                    this.card.labels = value.labels;
                    this.card.dueDate = value.dueDat;
                    
                }),
                debounceTime(300),
                takeUntil(this._unsubscribeAll)
            )
            .subscribe((value) => {

                // Update the card on the server
                //this._scrumboardService.updateCard(value.id, value).subscribe();

                // Mark for check
                //this._changeDetectorRef.markForCheck();
            });
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
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
