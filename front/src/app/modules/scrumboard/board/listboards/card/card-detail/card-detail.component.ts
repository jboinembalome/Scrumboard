import { Component, ElementRef, Inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, Observable, Subject, forkJoin, of } from 'rxjs';
import { catchError, debounceTime, map, mergeMap, startWith, switchMap, take, takeUntil, tap } from 'rxjs/operators';
import { ScrumboardService } from 'app/modules/scrumboard/scrumboard.service';
import { CdkScrollable } from '@angular/cdk/scrolling';
import { NgClass, AsyncPipe, DatePipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule, UntypedFormBuilder, UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatChipInputEvent, MatChipsModule } from '@angular/material/chips';
import { DateAdapter, MAT_DATE_FORMATS, MatOption } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDivider } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule, MatMenuTrigger } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AdherentSelectorComponent } from 'app/shared/components/adherent-selector/adherent-selector.component';
import { ActivitiesComponent } from './activities/activities.component';
import { ChecklistAddComponent } from './checklists/checklist/checklist-add/checklist-add.component';
import { ChecklistsComponent } from './checklists/checklists.component';
import { CommentAddComponent } from './comments/comment/comment-add/comment-add.component';
import { CommentsComponent } from './comments/comments.component';
import { LabelAddComponent } from './label/label-add/label-add.component';
import { LabelSelectorComponent } from './label/label-selector/label-selector.component';
import { LuxonDateAdapter } from '@angular/material-luxon-adapter';
import { ENTER } from '@angular/cdk/keycodes';
import { BlouppyConfirmationService } from 'app/shared/services/confirmation';
import { CardDetailDto, LabelDto, UserDto, ActivityDto, BoardsService, CardsService, TeamsService, UpdateCardCommand, ChecklistDto, CommentDto, BoardDetailDto, ActivitiesService, CommentsService } from 'app/swagger';
import { DateTime } from 'luxon';
import { BreadcrumbComponent } from 'app/shared/components/breadcrumb/breadcrumb.component';
import { Navigation } from 'app/core/navigation/models/navigation.model';

@Component({
    selector: 'scrumboard-card-detail',
    templateUrl: './card-detail.component.html',
    styleUrls: ['./card-detail.component.scss'],
    standalone: true,
    imports: [
      CdkScrollable,
      FormsModule,
      ReactiveFormsModule,
      NgClass,
      MatAutocompleteModule,
      MatButtonModule,
      MatChipsModule,
      MatDatepickerModule,
      MatDivider,
      MatFormFieldModule,
      MatIconModule,
      MatInputModule,
      MatMenuModule,
      MatOption,
      MatTooltipModule,
      BreadcrumbComponent,
      ChecklistsComponent,
      CommentAddComponent,
      CommentsComponent,
      ActivitiesComponent,
      AdherentSelectorComponent,
      LabelSelectorComponent,
      LabelAddComponent,
      ChecklistAddComponent,
      AsyncPipe,
      DatePipe,
    ],
    providers: [ 
      {
        provide: DateAdapter,
        useClass: LuxonDateAdapter
      },
      {
        provide : MAT_DATE_FORMATS,
        useValue: {
            parse  : {
                dateInput: 'D',
            },
            display: {
                dateInput         : 'DDD',
                monthYearLabel    : 'LLL yyyy',
                dateA11yLabel     : 'DD',
                monthYearA11yLabel: 'LLLL yyyy',
            },
        },
      }
    ]
})
export class CardDetailComponent implements OnInit, OnDestroy {
  private _unsubscribeAll: Subject<any> = new Subject<any>();
  id: any;
  boardId: any;
  board: BoardDetailDto;
  card: CardDetailDto;
  cardForm: UntypedFormGroup;

  urlAvatar: string = location.origin + "/api/adherents/avatar/";

  canModify: boolean;
  separatorKeysCodes: number[] = [ENTER];

  @ViewChild('checklistMenuTrigger') checklistMenu: MatMenuTrigger;

  /*** Labels ***/
  selectableLabel = true;
  removableLabel = true;
  labelCtrl = new UntypedFormControl();
  filteredLabels: Observable<LabelDto[]>;
  allLabels: LabelDto[] = [];
  comments: CommentDto[] = [];
  activities: ActivityDto[] = [];

  @ViewChild('labelInput') labelInput: ElementRef<HTMLInputElement>;

  /*** Members ***/
  selectableMember = true;
  removableMember = true;
  memberCtrl = new UntypedFormControl();
  filteredMembers: Observable<UserDto[]>;
  members: UserDto[] = [];
  allMembers: UserDto[] = [];

  @ViewChild('memberInput') memberInput: ElementRef<HTMLInputElement>;

  /*** Actitivies ***/
  activitiesEmitter$ = new BehaviorSubject<ActivityDto[]>(null);
  showActivities = false;

  /*** Breadcrumb ***/
  breadcrumbItems: Navigation[] = [];

  constructor(
    //public matDialogRef: MatDialogRef<DialogCardComponent>,
    private _activatedRoute: ActivatedRoute,
    private _boardsService: BoardsService,
    private _cardsService: CardsService,
    private _teamsService: TeamsService,
    private _activitiesService: ActivitiesService,
    private _commentsService: CommentsService,
    private _scrumboardService: ScrumboardService,
    private _blouppyConfirmationService: BlouppyConfirmationService,
    private _formBuilder: UntypedFormBuilder,
    private _router: Router) {

    this.id = this._activatedRoute.snapshot.paramMap.get('cardId');
    this.boardId = this._activatedRoute.snapshot.paramMap.get('boardId');

    this.filteredLabels = this.labelCtrl.valueChanges.pipe(
      startWith(<string>null),
      map((label: string | null) => label
        ? this.filterLabel(label)
        : this.allLabels.filter(label => this.card.labels.every(l => l.id !== label.id))));

    this.filteredMembers = this.memberCtrl.valueChanges.pipe(
      startWith(<string>null),
      map((member: string | null) => member
        ? this.filterMember(member)
        : this.allMembers.filter(member => this.card.assignees.every(m => m.id !== member.id))));
  }

  ngOnInit(): void {
    forkJoin({
      card: this._scrumboardService.card$
        .pipe(take(1)),
      board: this._scrumboardService.board$
        .pipe(take(1)),
      labels: this._boardsService.apiBoardsIdLabelsGet(this.boardId)
        .pipe(takeUntil(this._unsubscribeAll)),
      comments: this._commentsService.apiCardsCardIdCommentsGet(this.id)
        .pipe(takeUntil(this._unsubscribeAll)),
      activities: this._activitiesService.apiCardsCardIdActivitiesGet(this.id)
        .pipe(takeUntil(this._unsubscribeAll))
    }).pipe(
      mergeMap(({ card, board, labels, comments, activities }) => {
        this.card = card;
        this.board = board;
        this.allLabels = labels;
        this.comments = comments;
        this.activities = activities;
    
        // Now that this.card and this.board are initialized, call initializeBreadcrumb
        this.initializeBreadcrumb();
    
        if (this.activities) {
          this.activitiesEmitter$.next(this.activities);
        }
    
        // Prepare the card form
        this.cardForm = this._formBuilder.group({
          id: [''],
          name: ['', Validators.required],
          description: [''],
          labels: [[]],
          members: [[]],
          checklists: [[]],
          dueDate: [null]
        });

        this.initializeCardForm();

        this.handleCardFormValueChanges();
    
        return this._teamsService.apiTeamsIdGet(this.board.team.id)
          .pipe(takeUntil(this._unsubscribeAll));
      })
    ).subscribe({
      next: (members: UserDto[]) => {
        this.allMembers = members;
      },
      error: (error) => {
        console.error('Error:', error);
      }
    });
  }

  ngOnDestroy(): void {
    // Unsubscribe from all subscriptions
    this._unsubscribeAll.next(null);
    this._unsubscribeAll.complete();
  }

  deleteCard(card: CardDetailDto): void {
    // Open the confirmation dialog
    const confirmation = this._blouppyConfirmationService.open({
      title: 'Delete card',
      message: 'Are you sure you want to remove this card? This action is irreversible!',
      actions: {
        confirm: {
          label: 'Delete'
        }
      },
      dismissible: true
    });

    // Subscribe to the confirmation dialog closed action
    confirmation.afterClosed().subscribe((result) => {

      // If the confirm button pressed then remove the card
      if (result === 'confirmed') {
        this._cardsService.apiCardsIdDelete(card.id).subscribe(() => {
          this._router.navigate(['/scrumboard', this.boardId]);
        }, error => console.error(error));
      }
    });

  }

  addLabelChip(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();

    // Add our label
    if (value) {
      const index = this.allLabels.findIndex(l => l.name.toLowerCase() === value.toLowerCase());

      if (index >= 0) {
        this.addLabelToCard(this.allLabels[index]);
      }
      else {
        const newLabel: LabelDto = { name: value, colour: { colour: 'bg-gray-500' } };
        this.addLabelToCard(newLabel);
        this.allLabels.push(newLabel);
      }
    }

    // Clear the input value
    event.chipInput!.clear();

    this.labelCtrl.setValue(null);
  }

  removeLabelChip(label: LabelDto): void {
    this.removeLabelFromCard(label);

    // Hack to update the label list regarding the autocompletion
    this.labelCtrl.setValue(this.labelCtrl.value);
  }

  selectedLabelChip(event: MatAutocompleteSelectedEvent): void {
    this.card.labels.push(event.option.value);

    // Update the card form data
    this.cardForm.get('labels').patchValue(this.card.labels);

    this.labelInput.nativeElement.value = '';
    this.labelCtrl.setValue(null);
  }

  addLabel(label: LabelDto): void {
    let index = this.card.labels.findIndex(l => l.name === label.name);
    if (index < 0) {
      this.card.labels.push(label);

      // Update the card form data
      this.cardForm.get('labels').patchValue(this.card.labels);
    }

    index = this.allLabels.findIndex(l => l.name === label.name);
    if (index < 0)
      this.allLabels.push(label);

    this.labelCtrl.setValue(null);
  }

  updateLabels(labels: LabelDto[]): void {
    this.card.labels = labels;

    // Update the card form data
    this.cardForm.get('labels').patchValue(this.card.labels);

    this.labelCtrl.setValue(null);
  }

  deletedLabelFromCtrl(label: LabelDto): void {
    // Here we don't update the cardForm because it will update 
    // the card from the server but the card (and of course all the others 
    // that contain the deleted label) is already updated in the label-selector component.
    this.card.labels = this.card.labels.filter(l => l.id !== label.id);
  }

  removeMemberChip(member: UserDto): void {
    this.removeMemberFromCard(member);

    // Hack to update the member list regarding the autocompletion
    this.memberCtrl.setValue(this.memberCtrl.value);
  }

  selectedMemberChip(event: MatAutocompleteSelectedEvent): void {
    this.card.assignees.push(event.option.value);

    // Update the card form data
    this.cardForm.get('members').patchValue(this.card.assignees);

    this.memberInput.nativeElement.value = '';
    this.memberCtrl.setValue(null);
  }

  addMember(member: UserDto): void {
    const index = this.card.assignees.findIndex(m => m.id === member.id);
    if (index < 0)
      this.card.assignees.push(member);

    // Update the card form data
    this.cardForm.get('members').patchValue(this.card.assignees);

    this.memberCtrl.setValue(null);
  }

  updateMembers(assignees: UserDto[]): void {
    this.card.assignees = assignees;

    // Update the card form data
    this.cardForm.get('members').patchValue(this.card.assignees);

    this.memberCtrl.setValue(null);
  }

  addChecklist(checklist: ChecklistDto): void {
    this.card.checklists.push(checklist);

    this.cardForm.get('checklists').patchValue(this.card.checklists);

    this.checklistMenu.closeMenu();
  }


  updateChecklists(checklists: ChecklistDto[]): void {
    this.card.checklists = checklists;

    this.cardForm.get('checklists').patchValue(this.card.checklists);
  }


  addComment(comment: CommentDto): void {
    this.comments.push(comment);

    this._commentsService.apiCardsCardIdCommentsPost(this.card.id, { message: comment.message });
    
    // get the activities
    this._activitiesService.apiCardsCardIdActivitiesGet(this.card.id)
      .subscribe((activities: ActivityDto[]) => this.activitiesEmitter$.next(activities));
  }

  updateComments(comments: CommentDto[]): void {
    this.comments = comments;

    // get the activities
    this._activitiesService.apiCardsCardIdActivitiesGet(this.card.id)
    .subscribe((activities: ActivityDto[]) => this.activitiesEmitter$.next(activities));  
  }

  toogleActivities(): void {
    this.showActivities = !this.showActivities;

    if (this.showActivities) {
      // call api to get all activities in the card
    }
  }

  /**
   * Check if the given date is overdue
   */
  isOverdue(date: Date): boolean {
    return DateTime.fromISO(date).startOf('day') < DateTime.now().startOf('day');
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

  private initializeCardForm() {
    this.cardForm.setValue({
      id: this.card.id,
      name: this.card.name,
      description: this.card.description,
      labels: this.card.labels,
      members: this.card.assignees,
      checklists: this.card.checklists,
      dueDate: this.card.dueDate
    });
  }

  private handleCardFormValueChanges() {
    this.cardForm.valueChanges
        .pipe(
          tap((value) => {
            // Update the card object (Maybe use lodash? like below)
            //this.card = assign(this.card, value);

            this.card.id = value.id;
            this.card.name = value.name;
            this.card.description = value.description;
            this.card.labels = value.labels;
            this.card.assignees = value.members;
            this.card.checklists = value.checklists;
            this.card.dueDate = value.dueDate;
          }),
          debounceTime(300),
          takeUntil(this._unsubscribeAll)
        )
        .subscribe((value) => {
          this.updateCardOnServer(value);
        });
  }

  private updateCardOnServer(value: any) {
    const updateCardCommand: UpdateCardCommand = {
      id: value.id,
      name: value.name,
      description: value.description,
      suscribed: value.suscribed,
      dueDate: value.dueDate,
      labels: value.labels,
      assignees: value.members,
      checklists: value.checklists,
    };

    // Update the card on the server and get the activities
    this._cardsService.apiCardsIdPut(updateCardCommand.id, updateCardCommand)
      .pipe(
        switchMap((response: any) => this._activitiesService.apiCardsCardIdActivitiesGet(response.card.id)))
      .subscribe((activities: ActivityDto[]) => this.activitiesEmitter$.next(activities));
  }

  private filterLabel(value: string | LabelDto): LabelDto[] {
    const filterValue = (<LabelDto>value).name ? (<LabelDto>value).name.toLowerCase() : (<string>value).toLowerCase();

    return this.allLabels.filter(label => this.card.labels.every(l => l.name !== label.name) && label.name.toLowerCase().includes(filterValue));
  }

  private filterMember(value: string | UserDto): UserDto[] {
    const filterValue = (<UserDto>value).firstName ? (<UserDto>value).firstName.toLowerCase() : (<string>value).toLowerCase();

    return this.allMembers.filter(member => this.card.assignees.every(m => m.firstName !== member.firstName) && member.firstName.toLowerCase().includes(filterValue));
  }

  private addLabelToCard(label: LabelDto): void {
    this.card.labels.unshift(label);

    // Update the card form data
    this.cardForm.get('labels').patchValue(this.card.labels);
  }

  private removeLabelFromCard(label: LabelDto): void {
    const index = this.card.labels.findIndex(l => l === label);

    if (index >= 0) {
      this.card.labels.splice(index, 1);

      // Update the card form data
      this.cardForm.get('labels').patchValue(this.card.labels);
    }
  }

  private removeMemberFromCard(member: UserDto): void {
    const index = this.card.assignees.findIndex(m => m === member);

    if (index >= 0)
      this.card.assignees.splice(index, 1);

    // Update the card form data
    this.cardForm.get('members').patchValue(this.card.assignees);
  }

  private initializeBreadcrumb() {
    this.breadcrumbItems= [
      {
        icon: 'home',
        url: '/',
      },
      {
        name: 'Boards',
        url: '/scrumboard',
      },
      {
        name: this.board.name,
        url: `/scrumboard/${this.boardId}`,
      },
      {
        name: `Card-${this.id}`,
        url: `/scrumboard/${this.boardId}/card/${this.id}`,
      },
    ];
  }

}
