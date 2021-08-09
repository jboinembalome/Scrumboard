import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { StringColorPipe } from 'src/app/shared/pipes/string-color.pipe';
import { BoardDto, BoardsService } from 'src/app/swagger';
import { DataSourceSelectBoard } from './boards.constant';

@Component({
  selector: 'scrumboard-boards',
  templateUrl: './boards.component.html',
  styleUrls: ['./boards.component.scss']
})
export class BoardsComponent implements OnInit, OnDestroy {

  private boardsSubscription: Subscription;

  private isDesc: boolean = false;
  column: string = 'name';
  direction: number;

  dataSource = DataSourceSelectBoard;
  selectedSort = this.dataSource.sort[0].key;

  boards: BoardDto[];
  pinnedBoards: BoardDto[];
  filteredBoard: BoardDto[];

  boardColors: any[];

  searchText = '';

  memberMapping: { [k: string]: string } = { '=0': '# member', '=1': '# member', 'other': '# members' };

  constructor(
    private _stringColorPipe: StringColorPipe,
    private _boardsService: BoardsService,
    private _router: Router) {
  }

  ngOnInit(): void {
    this.boardsSubscription = this._boardsService.apiBoardsGet().subscribe(boards => {
      this.boards = boards;
      this.filteredBoard = boards;
      this.pinnedBoards = this.boards.filter(board => board.isPinned === true);
      this.boardColors = boards.map((board) => { return { key: board.boardSetting.colour.colour, bgColor: this._stringColorPipe.transform(board.boardSetting.colour) } })
        .filter((colour, index, self) => self.findIndex((c) => c.key === colour.key) === index);
    }, error => console.error(error));

    this.sort('most_recent');
  }

  ngOnDestroy() {
    if (this.boardsSubscription != undefined) {
      this.boardsSubscription.unsubscribe();
    }
  }

  /*
  * Updates the board pin.
  */
  updatePinned(name: string, isPinned: boolean) {
    let board = this.boards.find(b => b.name == name);

    if (isPinned) {
      board.isPinned = false;

      this.pinnedBoards = this.pinnedBoards.filter(b => b !== board);
    }
    else {
      board.isPinned = true;

      this.pinnedBoards.push(board);
    }
  }

  /*
  * Create a board.
  */
  createBoard(): void {
    this.boardsSubscription = this._boardsService.apiBoardsPost().subscribe(board => {
      this._router.navigate(['/scrumboard', board.board.id]);
    }, error => console.error(error));
  }

  /*
  * Deletes the board.
  */
  deleteBoard(board: BoardDto) {
    this.pinnedBoards = this.pinnedBoards.filter(b => b !== board);
    this.filteredBoard = this.filteredBoard.filter(b => b !== board);
    this.boards = this.boards.filter(b => b !== board);

    this.boardsSubscription = this._boardsService.apiBoardsIdDelete(board.id).subscribe(error => console.error(error));
  }

  /*
  * Sorts all boards.
  */
  sort(key: string) {
    switch (key) {
      case this.dataSource.sort[0].key: {
        this.isDesc = true;
        this.column = "lastActivity";
        break;
      }
      case this.dataSource.sort[1].key: {
        this.isDesc = false;
        this.column = "lastActivity";
        break;
      }
      case this.dataSource.sort[2].key: {
        this.isDesc = false;
        this.column = "name";
        break;
      }
      case this.dataSource.sort[3].key: {
        this.isDesc = true;
        this.column = "name";
        break;
      }
    }

    this.direction = this.isDesc ? -1 : 1;
  }

  /*
  * Filters all boards by colors.
  */
  filterColors(checkedValues: any[]) {
    if (checkedValues.length > 0) {
      this.filteredBoard = this.boards.filter(b => checkedValues.includes(b.boardSetting.colour.colour));
    }
    else {
      this.filteredBoard = this.boards;
    }

  }
}
