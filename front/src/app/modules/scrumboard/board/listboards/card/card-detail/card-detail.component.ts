import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ScrumboardService } from 'app/modules/scrumboard/scrumboard.service';
import { DialogCardComponent } from './dialog-card/dialog-card.component';


@Component({
    selector: 'scrumboard-card-detail',
    templateUrl: './card-detail.component.html',
    styleUrls: ['./card-detail.component.scss'],
    standalone: true
})
export class CardDetailComponent implements OnInit, OnDestroy {

  private _unsubscribeAll: Subject<any> = new Subject<any>();

  constructor(
    private _scrumboardService: ScrumboardService,
    private _activatedRoute: ActivatedRoute,
    private _matDialog: MatDialog,
    private _router: Router
  ) {
  }

  ngOnInit(): void {
    // Launch the modal
    this._matDialog.open(DialogCardComponent, {
      //disableClose: true, // Maybe use this
      panelClass: 'custom-dialog-container',
      data: {
        route: this._activatedRoute
      }
    })
      .afterClosed()
      .subscribe(() => {
        this._scrumboardService.getBoard(+this._activatedRoute.parent.snapshot.paramMap.get('boardId'))
          .pipe(takeUntil(this._unsubscribeAll))
          .subscribe(() => {
            // Go up twice because card routes are setup like this; "card/CARD_ID"
            this._router.navigate(['./../..'], { relativeTo: this._activatedRoute });

          });
      });
  }

  ngOnDestroy() {
    // Unsubscribe from all subscriptions
    this._unsubscribeAll.next(null);
    this._unsubscribeAll.complete();
  }
}
