import { Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';

@Component({
    selector: 'scrumboard-board-card-add',
    templateUrl: './card-add.component.html',
    standalone: true,
    imports: [
        MatButton,
        MatIcon,
        FormsModule,
    ],
})
export class CardAddComponent {
    @ViewChild('nameInput', { static: false })
    set nameInput(element: ElementRef<HTMLInputElement>) {
        if (element) {
            element.nativeElement.focus()
        }
    }
    
    @Input() buttonName: string = 'Add a card';
    @Output() saved: EventEmitter<string> = new EventEmitter<string>();

    cardName: string = '';
    editMode: boolean = false;

    constructor() {
    }

    saveCardName(): void {
        if (!this.cardName || this.cardName.trim() === '')
            return;

        this.saved.emit(this.cardName.trim());

        // Clear the new card name and hide the edit mode
        this.cardName = '';
        this.editMode = false;
    }

    /**
     * Toggle the visibility of the edit mode
     */
    toggleEditMode(): void {
        this.editMode = !this.editMode;
    }
}
