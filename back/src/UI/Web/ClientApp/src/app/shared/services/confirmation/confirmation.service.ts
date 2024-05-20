import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { merge } from 'lodash-es';
import { BlouppyConfirmationDialogComponent } from './dialog/dialog.component';
import { BlouppyConfirmationConfig } from './confirmation.types';

@Injectable()
export class BlouppyConfirmationService {
    private _defaultConfig: BlouppyConfirmationConfig = {
        title: 'Confirm action',
        message: 'Are you sure you want to confirm this action?',
        icon: {
            show: true,
            name: 'warning',
            color: 'warn'
        },
        actions: {
            confirm: {
                show: true,
                label: 'Confirm',
                color: 'warn'
            },
            cancel: {
                show: true,
                label: 'Cancel'
            }
        },
        dismissible: false
    };

    constructor(private _matDialog: MatDialog) {

    }

    open(config: BlouppyConfirmationConfig = {}): MatDialogRef<BlouppyConfirmationDialogComponent> {
        // Merge the user config with the default config
        const userConfig = merge({}, this._defaultConfig, config);

        // Open the dialog
        return this._matDialog.open(BlouppyConfirmationDialogComponent, {
            autoFocus: false,
            disableClose: !userConfig.dismissible,
            data: userConfig,
            panelClass: 'blouppy-confirmation-dialog-panel'
        });
    }
}
