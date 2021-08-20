import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import * as moment from 'moment';
import { CardDto, ListBoardDto } from 'src/app/swagger';

@Component({
  selector: 'scrumboard-listboards',
  templateUrl: './listboards.component.html',
  styleUrls: ['./listboards.component.scss']
})
export class ListBoardsComponent implements OnInit {
  private readonly _positionStep: number = 65536;
  private readonly _maxListCount: number = 200;
  private readonly _maxPosition: number = this._positionStep * 500;

  @Input() boardId: number;
  @Input() listBoards: ListBoardDto[];
  @Output() listBoardsChange = new EventEmitter<ListBoardDto[]>();

  constructor() {
  }

  ngOnInit(): void {
    // Sort the cards
    this.listBoards.forEach(listboard => listboard.cards.sort((a, b) => a.position - b.position));
  }

  /**
  * Calculates and sets the item positions
  * from given CdkDragDrop event.
  *
  * @param event
  */
  private _calculatePositions(event: CdkDragDrop<any[]>): any[] {
    // Get the items
    let items = event.container.data;
    const currentItem = items[event.currentIndex];
    const prevItem = items[event.currentIndex - 1] || null;
    const nextItem = items[event.currentIndex + 1] || null;

    // If the item moved to the top...
    if (!prevItem) {
      // If the item moved to an empty container
      if (!nextItem)
        currentItem.position = this._positionStep;
      else
        currentItem.position = nextItem.position / 2;
    }
    // If the item moved to the bottom...
    else if (!nextItem)
      currentItem.position = prevItem.position + this._positionStep;
    // If the item moved in between other items...
    else
      currentItem.position = (prevItem.position + nextItem.position) / 2;

    // Check if all item positions need to be updated
    if (!Number.isInteger(currentItem.position) || currentItem.position >= this._maxPosition) {
      // Re-calculate all orders
      items = items.map((value, index) => {
        value.position = (index + 1) * this._positionStep;
        return value;
      });

      // Return items
      return items;
    }

    // Return currentItem
    return [currentItem];
  }

  /**
  * Edits the list board name.
  *
  * @param event
  * @param list
  */
  editListBoardName(event: any, list: ListBoardDto): void {
    // Gets the target element
    const element: HTMLInputElement = event.target;

    // Gets the new name
    const newName = element.value;

    // If the name is empty...
    if (!newName || newName.trim() === '') {
      // Resets to original name and return
      element.value = list.name;
      return;
    }

    let listboard = this.listBoards.find(b => b.name == list.name);

    // Updates the list board name and element value
    listboard.name = element.value = newName.trim();

    this.listBoardsChange.emit(this.listBoards);
  }

  /**
  * Focus on the given element to start editing the list board name.
  *
  * @param listNameInput
  */
  renameListBoard(listNameInput: HTMLElement): void {
    // Uses timeout so it can wait for menu to close
    setTimeout(() => {
      listNameInput.focus();
    });
  }

  /**
  * Adds a new list board.
  *
  * @param name
  */
  addListBoard(name: string): void {
    // Limit the max list count
    if (this.listBoards.length >= this._maxListCount)
      return;

    const list: ListBoardDto = {
      name: name,
      position: this.listBoards.length ? this.listBoards[this.listBoards.length - 1].position + this._positionStep : this._positionStep,
      cards: [],
    };

    this.listBoards.push(list);

    this.listBoardsChange.emit(this.listBoards);
  }

  /**
  * Deletes the list board.
  *
  * @param id
  */
  deleteListBoard(id: number): void {
    // !!!! Don't forget to add a confirmation dialog.

    // Deletes the list
    const index = this.listBoards.findIndex(item => item.id === id);
    this.listBoards.splice(index, 1);

    this.listBoardsChange.emit(this.listBoards);
  }

  /**
  * Adds new card
  */
  addCard(listBoard: ListBoardDto, name: string): void {
    // Create a new card model
    const card : CardDto = {
      listBoardId: listBoard.id,
      position: listBoard.cards.length ? listBoard.cards[listBoard.cards.length - 1].position + this._positionStep : this._positionStep,
      name: name
    };

    listBoard.cards.push(card);

    this.listBoardsChange.emit(this.listBoards);
  }

  /**
  * List board dropped.
  *
  * @param event
  */
  listBoardDropped(event: CdkDragDrop<ListBoardDto[]>): void {
    // Moves the item
    moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);

    // Calculates the positions of the items
    const updatedListBoards = this._calculatePositions(event);

    updatedListBoards.forEach((updatedListBoard) => {
      const index = this.listBoards.findIndex(listBoard => listBoard.id === updatedListBoard.id);

      this.listBoards[index] = updatedListBoard;
    });

    this.listBoardsChange.emit(this.listBoards);
  }

  /**
  * Card dropped
  *
  * @param event
  */
  cardDropped(event: CdkDragDrop<CardDto[]>): void {
    // Move or transfer the item
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    }
    else {
      // Transfer the item inside another list board
      transferArrayItem(event.previousContainer.data, event.container.data, event.previousIndex, event.currentIndex);

      // Update the card's list it
      event.container.data[event.currentIndex].listBoardId = +event.container.id;
    }

    // Calculate the positions
    const updatedCards = this._calculatePositions(event);

    // Go through the updated cards
    updatedCards.forEach((updatedCard) => {
      const listBoardIndex = this.listBoards.findIndex(listBoard => listBoard.id === updatedCard.listBoardId);
      const cardIndex = this.listBoards[listBoardIndex].cards.findIndex(item => item.id === updatedCard.id);

      // Update the card
      this.listBoards[listBoardIndex].cards[cardIndex] = updatedCard;
    });

    this.listBoardsChange.emit(this.listBoards);
  }

  /**
  * Check if the given ISO_8601 date string is overdue
  *
  * @param date
  */
  isOverdue(date: string): boolean {
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
}
