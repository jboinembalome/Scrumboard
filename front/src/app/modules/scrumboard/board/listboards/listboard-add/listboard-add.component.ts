import { Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatIcon } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
    selector: 'scrumboard-board-listboard-add',
    templateUrl: './listboard-add.component.html',
    standalone: true,
    imports: [
        MatButtonModule,
        MatIcon,
        FormsModule,
    ],
})
export class ListBoardAddComponent {
    @ViewChild('nameInput', { static: false })
    set nameInput(element: ElementRef<HTMLInputElement>) {
        if (element) {
            element.nativeElement.focus()
        }
    }
    
    @Input() buttonName: string = 'Add a list';
    @Output() saved: EventEmitter<string> = new EventEmitter<string>();

    listBoardName: string = '';
    editMode: boolean = false;

    constructor() {
    }

    saveListBoardName(): void {
        if (!this.listBoardName || this.listBoardName.trim() === '')
            return;

        this.saved.emit(this.listBoardName.trim());

        // Clear the new list board name and hide the edit mode
        this.listBoardName = '';
        this.editMode = false;
    }

    /**
     * Toggle the visibility of the edit mode
     */
    toggleEditMode(): void {
        this.editMode = !this.editMode;
    }
}
