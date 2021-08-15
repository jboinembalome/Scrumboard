import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDrawer } from '@angular/material/sidenav';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { BoardDetailDto, UpdateBoardCommand, BoardsService } from 'src/app/swagger';
import { ListBoardDto } from 'src/app/swagger/model/listBoardDto';

@Component({
  selector: 'scrumboard-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.scss']
})
export class BoardComponent implements OnInit, OnDestroy {

  @ViewChild('matDrawer') matDrawer: MatDrawer;

  private boardSubscription: Subscription;

  id: any;
  board: BoardDetailDto;

  oldBoardName: string;
  isEditBoardName: boolean = false;

  constructor(
    private _activatedRoute: ActivatedRoute,
    private _boardsService: BoardsService) {
  }

  ngOnInit(): void {
    this.id = this._activatedRoute.snapshot.paramMap.get('id');

    this.boardSubscription = this._boardsService.apiBoardsIdGet(this.id).subscribe(board => {
      this.board = board;

      // Sort the board lists
      this.board.listBoards.sort((a, b) => a.position - b.position);
    }, error => console.error(error));
  }

  ngOnDestroy() {
    if (this.boardSubscription != undefined)
      this.boardSubscription.unsubscribe();
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
    const listboards: ListBoardDto[] = this.board.listBoards.map(listboard => ({
      id: listboard.id,
      name: listboard.name,
      position: listboard.position
    }));

    const updateBoardCommand: UpdateBoardCommand = {
      boardId: this.id,
      name: this.board.name,
      uri: this.board.uri,
      boardSetting: this.board.boardSetting,
      listBoards: listboards
    };

    this.boardSubscription = this._boardsService.apiBoardsIdPut(updateBoardCommand.boardId, updateBoardCommand).subscribe();
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
