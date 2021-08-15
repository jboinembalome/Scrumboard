import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ListBoardDetailDto } from 'src/app/swagger';

@Component({
  selector: 'scrumboard-listboards',
  templateUrl: './listboards.component.html',
  styleUrls: ['./listboards.component.scss']
})
export class ListBoardsComponent {

  private readonly _positionStep: number = 0;
  private readonly _maxListCount: number = 200;

  @Input() boardId: number;
  @Input() listBoards: ListBoardDetailDto[];
  @Output() listBoardsChange = new EventEmitter<ListBoardDetailDto[]>();

  constructor() {
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
    const previousItem = items[event.currentIndex - 1] || null;
    const nextItem = items[event.currentIndex + 1] || null;

    // If the item moved to the top...
    if (!previousItem) {
      // If the item moved to an empty container
      if (!nextItem)
        currentItem.position = this._positionStep;
      else
        currentItem.position = nextItem.position / 2;
    }
    // If the item moved to the bottom...
    else if (!nextItem)
      currentItem.position = previousItem.position + this._positionStep;
    // If the item moved in between other items...
    else
      currentItem.position = (previousItem.position + nextItem.position) / 2;

    // Check if all item positions need to be updated
    if (!Number.isInteger(currentItem.position) || currentItem.position >= (this._maxListCount - 1)) {
      // Re-calculate all orders
      items = items.map((value, index) => {
        value.position = index + this._positionStep;
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
  editListBoardName(event: any, list: ListBoardDetailDto): void {
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

    const list: ListBoardDetailDto = {
      name: name,
      position: this.listBoards.length ? this.listBoards[this.listBoards.length - 1].position + 1 : 0,
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
  * List board dropped.
  *
  * @param event
  */
  listBoardDropped(event: CdkDragDrop<ListBoardDetailDto[]>): void {
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
  * Tracks by function for ngFor loops.
  *
  * @param index
  * @param item
  */
  trackByFn(index: number, item: any): any {
    return item.id || index;
  }
}
