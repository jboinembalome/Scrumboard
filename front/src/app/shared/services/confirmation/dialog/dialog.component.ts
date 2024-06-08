import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogClose } from '@angular/material/dialog';
import { BlouppyConfirmationConfig } from '../confirmation.types';
import { NgClass } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { MatIconButton, MatButton } from '@angular/material/button';

@Component({
    selector: 'blouppy-confirmation-dialog',
    templateUrl: './dialog.component.html',
    styles: [
        /* language=SCSS */
        `
            .blouppy-confirmation-dialog-panel {
                @screen md {
                    @apply w-96;
                }

                .mat-dialog-container {
                    padding: 0 !important;
                    border-radius: 1rem;
                }
            }
        `
    ],
    encapsulation: ViewEncapsulation.None,
    standalone: true,
    imports: [MatIconButton, MatDialogClose, MatIcon, NgClass, MatButton]
})
export class BlouppyConfirmationDialogComponent implements OnInit {
    constructor(
        @Inject(MAT_DIALOG_DATA) public data: BlouppyConfirmationConfig,
        public matDialogRef: MatDialogRef<BlouppyConfirmationDialogComponent>) {
    }

    ngOnInit(): void {
    }
}
