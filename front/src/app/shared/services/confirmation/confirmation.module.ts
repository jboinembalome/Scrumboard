import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { BlouppyConfirmationService } from './confirmation.service';
import { BlouppyConfirmationDialogComponent } from './dialog/dialog.component';
import { CommonModule } from '@angular/common';

@NgModule({
    imports: [
        MatButtonModule,
        MatDialogModule,
        MatIconModule,
        CommonModule,
        BlouppyConfirmationDialogComponent
    ],
    providers: [
        BlouppyConfirmationService
    ]
})
export class BlouppyConfirmationModule {
    constructor(private _blouppyConfirmationService: BlouppyConfirmationService) {
    }
}
