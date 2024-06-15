import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { Subscription } from 'rxjs';
import { StringColorPipe } from 'app/shared/pipes/string-color.pipe';
import { BoardDto, BoardsService, UpdatePinnedBoardCommand } from 'app/swagger';
import { DataSourceSelectBoard } from './boards.constant';
import { BoardsFilterPipe } from './boards.pipe';
import { OrderByPipe } from '../../../shared/pipes/orderby.pipe';
import { DatePipe, I18nPluralPipe } from '@angular/common';
import { InputWithIconComponent } from '../../../shared/components/inputs/input-with-icon/input-with-icon.component';
import { SelectCheckboxComponent } from '../../../shared/components/selects/select-checkbox/select-checkbox.component';
import { SelectComponent } from '../../../shared/components/selects/select/select.component';
import { MatDivider } from '@angular/material/divider';
import { MatIcon } from '@angular/material/icon';
import { MatMenuTrigger, MatMenu } from '@angular/material/menu';
import { SimpleCardComponent } from '../../../shared/components/cards/simple-card/simple-card.component';
import { TitleComponent } from '../../../shared/components/title/title.component';
import { BlouppyUtils } from 'app/shared/utils/blouppyUtils';
import { MatButtonModule } from '@angular/material/button';

@Component({
    selector: 'scrumboard-boards',
    templateUrl: './boards.component.html',
    styleUrls: ['./boards.component.scss'],
    standalone: true,
    imports: [
      TitleComponent, 
      SimpleCardComponent, 
      RouterLink, 
      MatButtonModule,
      MatMenuTrigger, 
      MatIcon, 
      MatMenu, 
      MatDivider, 
      SelectComponent, 
      SelectCheckboxComponent, 
      InputWithIconComponent, 
      DatePipe, 
      I18nPluralPipe, 
      OrderByPipe, 
      BoardsFilterPipe]
})
export class BoardsComponent implements OnInit, OnDestroy {

  private boardsSubscription: Subscription;

  private isDesc: boolean = false;
  column: string = 'most_recent';
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
    private _boardsService: BoardsService,
    private _router: Router) {
  }

  ngOnInit(): void {
    this.boardsSubscription = this._boardsService.apiBoardsGet().subscribe(boards => {
      this.boards = boards;
      this.filteredBoard = boards;
      this.pinnedBoards = this.boards.filter(board => board.isPinned === true);
      this.boardColors = boards.map((board) => { return { key: board.boardSetting.colour.colour, bgColor: BlouppyUtils.formatColor(board.boardSetting.colour) } })
        .filter((colour, index, self) => self.findIndex((c) => c.key === colour.key) === index);
    }, error => console.error(error));

    this.sort('most_recent');
  }

  ngOnDestroy() {
    if (this.boardsSubscription != undefined)
      this.boardsSubscription.unsubscribe();
  }

  /*
  * Updates the board pin.
  */
  updatePinned(name: string, isPinned: boolean) {
    const board = this.boards.find(b => b.name == name);

    if (isPinned) {
      board.isPinned = false;

      this.pinnedBoards = this.pinnedBoards.filter(b => b !== board);
    }
    else {
      board.isPinned = true;

      this.pinnedBoards.push(board);
    }

    const updatePinnedBoardCommand: UpdatePinnedBoardCommand = { boardId: board.id, isPinned: board.isPinned };
    
    this.boardsSubscription = this._boardsService.apiBoardsIdPinnedPut(updatePinnedBoardCommand.boardId, updatePinnedBoardCommand).subscribe();
  }

  /*
  * Creates a board.
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
    if (checkedValues.length > 0)
      this.filteredBoard = this.boards.filter(b => checkedValues.includes(b.boardSetting.colour.colour));
    else
      this.filteredBoard = this.boards;
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
