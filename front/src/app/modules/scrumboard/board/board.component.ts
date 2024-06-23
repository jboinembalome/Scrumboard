import { Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatDrawer, MatDrawerContainer, MatDrawerContent } from '@angular/material/sidenav';
import { ActivatedRoute, RouterLink, RouterOutlet } from '@angular/router';
import { Observable, Subject, Subscription } from 'rxjs';
import { map, takeUntil } from 'rxjs/operators';
import { IUser } from 'app/core/auth/models/user.model';
import { AuthService } from 'app/core/auth/services/auth.service';
import { BoardDetailDto, UpdateBoardCommand, BoardsService, ListBoardDto, CardDto, AdherentDto, UpdateTeamCommand, TeamsService, AdherentsService } from 'app/swagger';
import { ScrumboardService } from '../scrumboard.service';
import { InitialPipe } from '../../../shared/pipes/initial.pipe';
import { AsyncPipe } from '@angular/common';
import { ListBoardsComponent } from './listboards/listboards.component';
import { CdkScrollable } from '@angular/cdk/scrolling';
import { AdherentSelectorComponent } from '../../../shared/components/adherent-selector/adherent-selector.component';
import { MatMenuTrigger, MatMenu } from '@angular/material/menu';
import { MatTooltip } from '@angular/material/tooltip';
import { MatIcon } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { SettingComponent } from './setting/setting.component';

@Component({
    selector: 'scrumboard-board',
    templateUrl: './board.component.html',
    styleUrls: ['./board.component.scss'],
    encapsulation: ViewEncapsulation.None,
    standalone: true,
    imports: [
      MatDrawerContainer, 
      MatDrawer, 
      SettingComponent, 
      MatDrawerContent, 
      MatButtonModule, 
      MatIcon, 
      RouterLink, 
      MatTooltip, 
      MatMenuTrigger, 
      MatMenu, 
      AdherentSelectorComponent, 
      CdkScrollable, 
      ListBoardsComponent, 
      RouterOutlet, 
      AsyncPipe, 
      InitialPipe
    ]
})
export class BoardComponent implements OnInit, OnDestroy {

  @ViewChild('matDrawer') matDrawer: MatDrawer;

  private boardSubscription: Subscription;
  private _unsubscribeAll: Subject<any> = new Subject<any>();

  id: any;
  board: BoardDetailDto;
  currentUser: Observable<IUser>;

  allAdherents: Observable<AdherentDto[]>;

  oldBoardName: string;
  isEditBoardName: boolean = false;

  urlAvatar: string = location.origin + "/api/adherents/avatar/";

  constructor(
    private _activatedRoute: ActivatedRoute,
    private _scrumboardService: ScrumboardService,
    private _boardsService: BoardsService,
    private _teamsService: TeamsService,
    private _adherentsService: AdherentsService,
    private _authService: AuthService) {
  }

  ngOnInit(): void {
    this.id = this._activatedRoute.snapshot.paramMap.get('boardId');

    // Get the board
    this._scrumboardService.board$
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe((board: BoardDetailDto) => {
        this.board = { ...board };

        // Sort the board lists
        this.board.listBoards.sort((a, b) => a.position - b.position);
      });

    // Get the current user
    this.currentUser = this._authService.getUser();
    // Get all the adherents
    this.allAdherents = this._adherentsService.apiAdherentsGet()
      .pipe(map(a => a.filter(a => a.id !== this.board.adherent.id)));
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
  editBoardName(event: any, board: BoardDetailDto): void {
    // Gets the target element
    const element: HTMLInputElement = event.target;

    // Gets the new name
    const newName = element.value;

    // If the name is empty
    if (!newName || newName.trim() === '') {
      // Keep to original name
      element.value = board.name;
      return;
    }

    this.updateBoard();
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
  * Updates the team board.
  */
  updateTeam(adherents: AdherentDto[]): void {
    if (!adherents.some(a => a.id === this.board.adherent.id))
      adherents.push(this.board.adherent);

    const updateTeamCommand: UpdateTeamCommand = {
      id: this.board.team.id,
      adherents: adherents
    };

    // Update the team on the server
    this._teamsService.apiTeamsIdPut(this.board.team.id, updateTeamCommand)
      .subscribe(
        () => this.board.team.adherents = adherents,
        error => console.error(error));

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
