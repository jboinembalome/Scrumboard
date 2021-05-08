import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { fuseAnimations } from '@fuse/animations';
import { FuseTranslationLoaderService } from '@fuse/services/translation-loader.service';

import { TranslateService } from '@ngx-translate/core';

import { ScrumboardService } from 'app/main/apps/scrumboard/scrumboard.service';
import { Board } from 'app/main/apps/scrumboard/board.model';

import { locale as english } from 'app/main/apps/scrumboard//i18n/en';
import { locale as french } from 'app/main/apps/scrumboard//i18n/fr';

@Component({
    selector     : 'scrumboard',
    templateUrl  : './scrumboard.component.html',
    styleUrls    : ['./scrumboard.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations   : fuseAnimations
})
export class ScrumboardComponent implements OnInit, OnDestroy
{
    boards: any[];

    // Private
    private _unsubscribeAll: Subject<any>;

    /**
     * Constructor
     *
     * @param {Router} _router
     * @param {ScrumboardService} _scrumboardService
     * @param {FuseTranslationLoaderService} _fuseTranslationLoaderService
     * @param {TranslateService} translate
     */
    constructor(
        private  _router: Router,
        private _scrumboardService: ScrumboardService,
        private _fuseTranslationLoaderService: FuseTranslationLoaderService,
        private translate: TranslateService
    )
    {
        // Load the translations
        this._fuseTranslationLoaderService.loadTranslations(english, french);

        // Set the private defaults
        this._unsubscribeAll = new Subject();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void
    {
        this._scrumboardService.onBoardsChanged
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(boards => {
                this.boards = boards;
            });
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void
    {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * New board
     */
    newBoard(): void
    {
        let untitle_board: string;

        this.translate.get('SCRUMBOARD.UNTITLED_BOARD')
        .subscribe(val => untitle_board = val);

        const newBoard = new Board({ name : untitle_board });

        this._scrumboardService.createNewBoard(newBoard).then(() => {
            this._router.navigate(['/apps/scrumboard/boards/' + newBoard.id + '/' + newBoard.uri]);
        });
    }
}
