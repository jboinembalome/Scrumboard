import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { DialogCardComponent } from './dialog-card/dialog-card.component';


@Component({
  selector: 'scrumboard-card-detail',
  templateUrl: './card-detail.component.html',
  styleUrls: ['./card-detail.component.scss']

})
export class CardDetailComponent implements OnInit {

  constructor(
    private _activatedRoute: ActivatedRoute,
    private _matDialog: MatDialog,
    private _router: Router
  ) {
  }

  ngOnInit(): void
  {
      // Launch the modal
      this._matDialog.open(DialogCardComponent, {
        //disableClose: true, // Maybe use this
        panelClass: 'custom-dialog-container', 
      })
          .afterClosed()
          .subscribe(() => {

              // Go up twice because card routes are setup like this; "card/CARD_ID"
              this._router.navigate(['./../..'], {relativeTo: this._activatedRoute});
          });
  }

}
