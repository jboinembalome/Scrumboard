import { ENTER } from '@angular/cdk/keycodes';
import { Component, ElementRef, Inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable, Subject } from 'rxjs';
import { tap, debounceTime, takeUntil, startWith, map, mergeMap } from 'rxjs/operators';
import { BoardDetailDto, CardDetailDto, CardsService, ChecklistDto, CommentDto, LabelDto, LabelsService, UpdateCardCommand } from 'src/app/swagger';
import * as moment from 'moment';
import { MatMenuTrigger } from '@angular/material/menu';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';
import { AuthService } from 'src/app/core/auth/services/auth.service';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'scrumboard-dialog-card',
  templateUrl: './dialog-card.component.html',
})
export class DialogCardComponent implements OnInit, OnDestroy {

  private _unsubscribeAll: Subject<any> = new Subject<any>();
  id: any;
  card: CardDetailDto;
  cardForm: FormGroup;

  canModify: boolean;

  @ViewChild('checklistMenuTrigger') checklistMenu: MatMenuTrigger;

  /*** Labels ***/
  selectableLabel = true;
  removableLabel = true;
  separatorKeysCodes: number[] = [ENTER];
  labelCtrl = new FormControl();
  filteredLabels: Observable<LabelDto[]>;
  allLabels: LabelDto[] = [];
  @ViewChild('labelInput') labelInput: ElementRef<HTMLInputElement>;

  /*** Members ***/
  selectableMember = true;
  removableMember = true;
  memberCtrl = new FormControl();
  filteredMembers: Observable<any[]>;
  members: any[] = [
    //{ name: 'Jimmy', avatar: 'assets/images/avatars/jimmy.jpg' }
  ];
  allMembers: any[] = [
    { name: 'adherent@localhost', job: 'Software Ingineer', avatar: 'assets/images/avatars/jimmy.jpg' },
    { name: 'Gwendoline', job: 'Software Tester', avatar: 'assets/images/avatars/gwendoline.jpg' },
    { name: 'Guyliane', job: 'Software Ingineer', avatar: 'assets/images/avatars/guyliane.jpg' },
    { name: 'Corentin', job: 'Systems and Networks Engineer', avatar: 'assets/images/avatars/corentin.jpg' },
    { name: 'Patrice', job: 'Software Ingineer', avatar: 'assets/images/avatars/patrice.jpg' },
    { name: 'Cédric', job: 'Software Tester', avatar: 'assets/images/avatars/profile.jpg' },
  ];
  @ViewChild('memberInput') memberInput: ElementRef<HTMLInputElement>;

  constructor(
    public matDialogRef: MatDialogRef<DialogCardComponent>,
    @Inject(MAT_DIALOG_DATA) data: { route: ActivatedRoute },
    private _labelsService: LabelsService,
    private _cardsService: CardsService,
    private _formBuilder: FormBuilder) {
    this.id = data.route.snapshot.paramMap.get('cardId');

    this.filteredLabels = this.labelCtrl.valueChanges.pipe(
      startWith(<string>null),
      map((label: string | null) => label
        ? this.filterLabel(label)
        : this.allLabels.filter(label => this.card.labels.every(l => l.name !== label.name))));

    this.filteredMembers = this.memberCtrl.valueChanges.pipe(
      startWith(<string>null),
      map((member: string | null) => member
        ? this.filterMember(member)
        : this.allMembers.filter(member => this.members.every(m => m.name !== member.name))));
  }

  ngOnInit(): void {
    /*         // Get the board
            this._scrumboardService.board$
                .pipe(takeUntil(this._unsubscribeAll))
                .subscribe((board) => {
    
                    // Board data
                    this.board = board;
    
                    // Get the labels
                    this.labels = this.filteredLabels = board.labels;
                });
     */

    // Prepare the card form
    this.cardForm = this._formBuilder.group({
      id: [''],
      name: ['', Validators.required],
      description: [''],
      labels: [[]],
      checklists: [[]],
      comments: [[]],
      dueDate: [null]
    });

    // Get the card details
    this._cardsService.apiCardsIdGet(this.id).pipe(
      mergeMap((card) => {
        this.card = card;

        // Fill the form
        this.cardForm.setValue({
          id: this.card.id,
          name: this.card.name,
          description: this.card.description,
          labels: this.card.labels,
          checklists: this.card.checklists,
          comments: this.card.comments,
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
              adherents: null,
              attachments: null,
              checklists: value.checklists,
              comments: value.comments
            };

            // Update the card on the server
            this._cardsService.apiCardsIdPut(updateCardCommand.id, updateCardCommand).subscribe(() => {
            }, error => console.error(error));
          });

        // Get all labels in board
        return this._labelsService.apiLabelsBoardsBoardIdGet(card.boardId);
      })
    ).subscribe((labels) => {
      this.allLabels = labels;
    });


  }

  ngOnDestroy(): void {
    // Unsubscribe from all subscriptions
    this._unsubscribeAll.next();
    this._unsubscribeAll.complete();
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

  addMemberChip(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();

    // Add our member
    if (value) {
      const index = this.allMembers.findIndex(m => m.name.toLowerCase() === value.toLowerCase());

      if (index >= 0)
        this.addMemberToCard(this.allMembers[index]);
    }

    // Clear the input value
    event.chipInput!.clear();

    this.memberCtrl.setValue(null);
  }

  removeMemberChip(member: any): void {
    this.removeMemberFromCard(member);

    // Hack to update the member list regarding the autocompletion
    this.memberCtrl.setValue(this.memberCtrl.value);
  }

  selectedMemberChip(event: MatAutocompleteSelectedEvent): void {
    this.members.push(event.option.value);
    this.memberInput.nativeElement.value = '';
    this.memberCtrl.setValue(null);
  }

  addMember(member: any): void {
    const index = this.members.findIndex(m => m.name === member.name);
    if (index < 0)
      this.members.push(member);

    this.memberCtrl.setValue(null);
  }

  updateMembers(members: any[]): void {
    this.members = members;
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

    this.cardForm.get('comments').patchValue(this.card.comments);
  }

  updateComments(comments: CommentDto[]): void {
    this.card.comments = comments;

    this.cardForm.get('comments').patchValue(this.card.comments);
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

  private filterMember(value: string | any): LabelDto[] {
    const filterValue = (<any>value).name ? (<any>value).name.toLowerCase() : (<string>value).toLowerCase();

    return this.allMembers.filter(member => this.card.labels.every(m => m.name !== member.name) && member.name.toLowerCase().includes(filterValue));
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

  private addMemberToCard(member: any): void {
    this.members.unshift(member);
  }

  private removeMemberFromCard(member: LabelDto): void {
    const index = this.members.findIndex(m => m === member);

    if (index >= 0)
      this.members.splice(index, 1);
  }
}
