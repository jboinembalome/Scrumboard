import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { BoardDetailDto, UpdateBoardCommand, BoardsService } from 'src/app/swagger';

@Component({
  selector: 'scrumboard-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.scss']
})
export class BoardComponent implements OnInit, OnDestroy {
  private boardSubscription: Subscription;

  board: BoardDetailDto;
  updateBoardCommand: UpdateBoardCommand;
  id: any;
  oldTitle: string;
  isEditTitle: boolean = false;

  constructor(
    private _activatedRoute: ActivatedRoute,
    private _boardsService: BoardsService) {
  }

  ngOnInit(): void {
    this.id = this._activatedRoute.snapshot.paramMap.get('id');

    this.boardSubscription = this._boardsService.apiBoardsIdGet(this.id).subscribe(board => {
      this.board = board;
    }, error => console.error(error));
  }

  ngOnDestroy() {
    if (this.boardSubscription != undefined) {
      this.boardSubscription.unsubscribe();
    }
  }

  /**
  * Edits the board title.
  */
  editTitle(): void {
    this.isEditTitle = !this.isEditTitle;
    this.oldTitle = this.board.name;

    if (!this.isEditTitle) {
      this.updateBoard();
    }
  }

  /**
  * Keeps the old board title.
  */
  keepOldTitle(): void {
    this.isEditTitle = !this.isEditTitle;
    this.board.name = this.oldTitle;
  }

  /**
  * Updates the board.
  */
  updateBoard(): void {
    this.updateBoardCommand = { boardId: this.id, name: this.board.name };

    this.boardSubscription = this._boardsService.apiBoardsIdPut(this.updateBoardCommand.boardId, this.updateBoardCommand).subscribe(error => console.error(error));
  }
}
