import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDrawer } from '@angular/material/sidenav';
import { ActivatedRoute } from '@angular/router';
import { Subject, Subscription } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { BoardDetailDto, UpdateBoardCommand, BoardsService, ListBoardDto, CardDto } from 'src/app/swagger';
import { ScrumboardService } from '../scrumboard.service';

@Component({
  selector: 'scrumboard-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.scss']
})
export class BoardComponent implements OnInit, OnDestroy {

  @ViewChild('matDrawer') matDrawer: MatDrawer;

  private boardSubscription: Subscription;
  private _unsubscribeAll: Subject<any> = new Subject<any>();

  id: any;
  board: BoardDetailDto;

  oldBoardName: string;
  isEditBoardName: boolean = false;

  constructor(
    private _activatedRoute: ActivatedRoute,
    private _scrumboardService: ScrumboardService,
    private _boardsService: BoardsService) {
  }

  ngOnInit(): void {
    this.id = this._activatedRoute.snapshot.paramMap.get('boardId');

    // Get the board
    this._scrumboardService.board$
    .pipe(takeUntil(this._unsubscribeAll))
    .subscribe((board: BoardDetailDto) => {
        this.board = {...board};

        // Sort the board lists
        this.board.listBoards.sort((a, b) => a.position - b.position);
    });
  }

  ngOnDestroy() {
    // Unsubscribe from all subscriptions
    this._unsubscribeAll.next(null);
    this._unsubscribeAll.complete();

    if (this.boardSubscription != undefined)
      this.boardSubscription.unsubscribe();
  }

  /**
   * Updates the id of listBoards that are undefined.
   * @param listBoardsToBeUpdated 
   * @param updatedListBoards 
   */
  private updateListBoardsId(listBoardsToBeUpdated: ListBoardDto[], updatedListBoards: ListBoardDto[]): void {
    updatedListBoards.forEach((updatedListBoard) => {
      const listBoardIndex = listBoardsToBeUpdated.findIndex(listBoard => listBoard.name === updatedListBoard.name);

      if (!this.board.listBoards[listBoardIndex].id)
        listBoardsToBeUpdated[listBoardIndex].id = updatedListBoard.id;

      this.updateCardsId(listBoardIndex, listBoardsToBeUpdated, updatedListBoard.cards);
    });
  }

  /**
   * Updates the id of cards that are undefined.
   * @param listBoardIndex 
   * @param listBoardsToBeUpdated 
   * @param updatedCards 
   */
  private updateCardsId(listBoardIndex: number, listBoardsToBeUpdated: ListBoardDto[], updatedCards: CardDto[]): void {
    updatedCards.forEach((updatedCard: { name: string; id: number; }) => {
      const cardIndex = listBoardsToBeUpdated[listBoardIndex].cards.findIndex(card => card.name === updatedCard.name);

      if (!listBoardsToBeUpdated[listBoardIndex].cards[cardIndex].id)
        listBoardsToBeUpdated[listBoardIndex].cards[cardIndex].id = updatedCard.id;
    });
  }

  /**
  * Edits the board name.
  */
  editBoardName(): void {
    this.isEditBoardName = !this.isEditBoardName;

    if (!this.isEditBoardName) {
      // If the name is empty...
      if (!this.board.name || this.board.name.trim() === '') {
        // Resets to original name and return
        this.board.name = this.oldBoardName;
        return;
      }

      this.updateBoard();
      return;
    }

    this.oldBoardName = this.board.name;
  }

  /**
  * Keeps the old board name.
  */
  keepOldBoardName(): void {
    this.isEditBoardName = !this.isEditBoardName;
    this.board.name = this.oldBoardName;
  }

  /**
  * Updates the board.
  */
  updateBoard(): void {
    const updateBoardCommand: UpdateBoardCommand = {
      boardId: this.id,
      name: this.board.name,
      uri: this.board.uri,
      boardSetting: this.board.boardSetting,
      listBoards: this.board.listBoards
    };

    this.boardSubscription = this._boardsService.apiBoardsIdPut(updateBoardCommand.boardId, updateBoardCommand).subscribe(response => {
      this.updateListBoardsId(this.board.listBoards, response.listBoards);
    }, error => console.error(error));
  }

  /**
  * Open the setting panel.
  */
  openSettingPanel(): void {
    this.matDrawer.open();
  }

  /**
  * Close the setting panel.
  */
  closeSettingPanel(): void {
    this.matDrawer.close();
  }
}
