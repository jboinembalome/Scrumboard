import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BlouppyConfirmationConfig } from '../confirmation.types';

@Component({
    selector: 'blouppy-confirmation-dialog',
    templateUrl: './dialog.component.html',
    styles       : [
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
    encapsulation: ViewEncapsulation.None
})
export class BlouppyConfirmationDialogComponent implements OnInit {
    constructor(
        @Inject(MAT_DIALOG_DATA) public data: BlouppyConfirmationConfig,
        public matDialogRef: MatDialogRef<BlouppyConfirmationDialogComponent>) {
    }

    ngOnInit(): void {
    }
}
