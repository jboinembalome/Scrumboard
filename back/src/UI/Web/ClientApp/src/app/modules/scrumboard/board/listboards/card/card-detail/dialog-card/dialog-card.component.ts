import { ENTER } from '@angular/cdk/keycodes';
import { Component, ElementRef, Inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BehaviorSubject, forkJoin, Observable, Subject } from 'rxjs';
import { tap, debounceTime, takeUntil, startWith, map, mergeMap, flatMap, switchMap } from 'rxjs/operators';
import { AdherentDto, CardDetailDto, CardsService, ChecklistDto, CommentDto, LabelDto, BoardsService, TeamsService, UpdateCardCommand, ActivityDto } from 'src/app/swagger';
import * as moment from 'moment';
import { MatMenuTrigger } from '@angular/material/menu';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';
import { ActivatedRoute } from '@angular/router';
import { ScrumboardService } from 'src/app/modules/scrumboard/scrumboard.service';
import { BlouppyConfirmationService } from 'src/app/shared/services/confirmation';


@Component({
  selector: 'scrumboard-dialog-card',
  templateUrl: './dialog-card.component.html',
})
export class DialogCardComponent implements OnInit, OnDestroy {

  private _unsubscribeAll: Subject<any> = new Subject<any>();
  id: any;
  boardId: any;
  card: CardDetailDto;
  cardForm: FormGroup;

  urlAvatar: string = location.origin + "/api/adherents/avatar/";

  canModify: boolean;
  separatorKeysCodes: number[] = [ENTER];

  @ViewChild('checklistMenuTrigger') checklistMenu: MatMenuTrigger;

  /*** Labels ***/
  selectableLabel = true;
  removableLabel = true;
  labelCtrl = new FormControl();
  filteredLabels: Observable<LabelDto[]>;
  allLabels: LabelDto[] = [];
  @ViewChild('labelInput') labelInput: ElementRef<HTMLInputElement>;

  /*** Members ***/
  selectableMember = true;
  removableMember = true;
  memberCtrl = new FormControl();
  filteredMembers: Observable<AdherentDto[]>;
  members: AdherentDto[] = [];
  allMembers: AdherentDto[] = [];

  @ViewChild('memberInput') memberInput: ElementRef<HTMLInputElement>;

  /*** Actitivies ***/
  activitiesEmitter$ = new BehaviorSubject<ActivityDto[]>(null);
  showActivities = false;
  constructor(
    public matDialogRef: MatDialogRef<DialogCardComponent>,
    @Inject(MAT_DIALOG_DATA) data: { route: ActivatedRoute },
    private _boardsService: BoardsService,
    private _cardsService: CardsService,
    private _teamsService: TeamsService,
    private _scrumboardService: ScrumboardService,
    private _blouppyConfirmationService: BlouppyConfirmationService,
    private _formBuilder: FormBuilder) {
    this.id = data.route.snapshot.paramMap.get('cardId');
    this.boardId = data.route.parent.snapshot.paramMap.get('boardId');
    this.filteredLabels = this.labelCtrl.valueChanges.pipe(
      startWith(<string>null),
      map((label: string | null) => label
        ? this.filterLabel(label)
        : this.allLabels.filter(label => this.card.labels.every(l => l.id !== label.id))));

    this.filteredMembers = this.memberCtrl.valueChanges.pipe(
      startWith(<string>null),
      map((member: string | null) => member
        ? this.filterMember(member)
        : this.allMembers.filter(member => this.card.adherents.every(m => m.id !== member.id))));
  }

  ngOnInit(): void {
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

    forkJoin(
      this._cardsService.apiCardsIdGet(this.id),
      this._boardsService.apiBoardsIdLabelsGet(this.boardId),
    ).pipe(mergeMap(([card, labels]) => {
      this.card = card;

      if (this.card.activities)
        this.activitiesEmitter$.next(this.card.activities);

      // Fill the form
      this.cardForm.setValue({
        id: this.card.id,
        name: this.card.name,
        description: this.card.description,
        labels: this.card.labels,
        members: this.card.adherents,
        checklists: this.card.checklists,
        dueDate: this.card.dueDate
      });

      // Update card when there is a value change on the card form
      this.cardForm.valueChanges
        .pipe(
          tap((value) => {
            // Update the card object (Maybe use lodash? like below)
            //this.card = assign(this.card, value);

            this.card.id = value.id;
            this.card.name = value.name;
            this.card.description = value.description;
            this.card.labels = value.labels;
            this.card.adherents = value.members;
            this.card.checklists = value.checklists;
            this.card.dueDate = value.dueDate;
          }),
          debounceTime(300),
          takeUntil(this._unsubscribeAll)
        )
        .subscribe((value) => {
          const updateCardCommand: UpdateCardCommand = {
            id: value.id,
            name: value.name,
            description: value.description,
            suscribed: value.suscribed,
            dueDate: value.dueDate,
            labels: value.labels,
            adherents: value.members,
            attachments: null,
            checklists: value.checklists,
          };

          // Update the card on the server and get the activities
          this._cardsService.apiCardsIdPut(updateCardCommand.id, updateCardCommand)
            .pipe(
              switchMap((response: any) => this._cardsService.apiCardsIdActivitiesGet(response.card.id)))
            .subscribe((activities: ActivityDto[]) => this.activitiesEmitter$.next(activities));
        });

      this.allLabels = labels;

      return this._scrumboardService.board$
        .pipe(mergeMap(b => this._teamsService.apiTeamsIdGet(b.team.id)));
    })).subscribe((adherents) => {
      this.allMembers = adherents;
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
          this.matDialogRef.close();
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
        const newLabel: LabelDto = { name: value, colour: { colour: 'bg-gray-500' }, cardIds: [this.id] };
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

  removeMemberChip(member: AdherentDto): void {
    this.removeMemberFromCard(member);

    // Hack to update the member list regarding the autocompletion
    this.memberCtrl.setValue(this.memberCtrl.value);
  }

  selectedMemberChip(event: MatAutocompleteSelectedEvent): void {
    this.card.adherents.push(event.option.value);

    // Update the card form data
    this.cardForm.get('members').patchValue(this.card.adherents);

    this.memberInput.nativeElement.value = '';
    this.memberCtrl.setValue(null);
  }

  addMember(member: AdherentDto): void {
    const index = this.card.adherents.findIndex(m => m.id === member.id);
    if (index < 0)
      this.card.adherents.push(member);

    // Update the card form data
    this.cardForm.get('members').patchValue(this.card.adherents);

    this.memberCtrl.setValue(null);
  }

  updateMembers(members: AdherentDto[]): void {
    this.card.adherents = members;

    // Update the card form data
    this.cardForm.get('members').patchValue(this.card.adherents);

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
    this.card.comments.push(comment);
    
    // get the activities
    this._cardsService.apiCardsIdActivitiesGet(this.card.id)
      .subscribe((activities: ActivityDto[]) => this.activitiesEmitter$.next(activities));
  }

  updateComments(comments: CommentDto[]): void {
    this.card.comments = comments;

    // get the activities
    this._cardsService.apiCardsIdActivitiesGet(this.card.id)
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
    return moment(date, moment.ISO_8601).isBefore(moment(), 'days');
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

  private filterLabel(value: string | LabelDto): LabelDto[] {
    const filterValue = (<LabelDto>value).name ? (<LabelDto>value).name.toLowerCase() : (<string>value).toLowerCase();

    return this.allLabels.filter(label => this.card.labels.every(l => l.name !== label.name) && label.name.toLowerCase().includes(filterValue));
  }

  private filterMember(value: string | AdherentDto): AdherentDto[] {
    const filterValue = (<AdherentDto>value).firstName ? (<AdherentDto>value).firstName.toLowerCase() : (<string>value).toLowerCase();

    return this.allMembers.filter(member => this.card.adherents.every(m => m.firstName !== member.firstName) && member.firstName.toLowerCase().includes(filterValue));
  }

  private addLabelToCard(label: LabelDto): void {
    if (label.cardIds.indexOf(this.id) === -1)
      label.cardIds.push(this.id);

    this.card.labels.unshift(label);

    // Update the card form data
    this.cardForm.get('labels').patchValue(this.card.labels);
  }

  private removeLabelFromCard(label: LabelDto): void {
    const index = this.card.labels.findIndex(l => l === label);

    if (index >= 0) {
      const cardIdIndex = label.cardIds.indexOf(this.id);
      if (cardIdIndex === -1)
        label.cardIds.splice(cardIdIndex, 1);

      this.card.labels.splice(index, 1);

      // Update the card form data
      this.cardForm.get('labels').patchValue(this.card.labels);
    }
  }

  private removeMemberFromCard(member: LabelDto): void {
    const index = this.card.adherents.findIndex(m => m === member);

    if (index >= 0)
      this.card.adherents.splice(index, 1);

    // Update the card form data
    this.cardForm.get('members').patchValue(this.card.adherents);
  }
}
