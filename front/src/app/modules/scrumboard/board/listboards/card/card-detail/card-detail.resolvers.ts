import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';
import { Observable, forkJoin, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { BoardDetailDto, CardDetailDto } from 'app/swagger';
import { ScrumboardService } from 'app/modules/scrumboard/scrumboard.service';

@Injectable({
    providedIn: 'root'
})
export class CardDetailResolver 
{
    constructor(
        private _router: Router, 
        private _scrumboardService: ScrumboardService) {
    }
    
    /**
     * Resolver
     *
     * @param route
     * @param state
     */
    resolve(
        route: ActivatedRouteSnapshot, 
        state: RouterStateSnapshot) 
        : Observable<{ card: CardDetailDto; board: BoardDetailDto; }> {
        return forkJoin({
            card: this._scrumboardService.getCard(+route.paramMap.get('cardId')),
            board: this._scrumboardService.getBoard(+route.paramMap.get('boardId'))
        })
        .pipe(catchError(error => {
            // Log the error
            console.error(error);

            // Get the parent url
            const parentUrl = state.url.split('/').slice(0, -1).join('/');

            // Navigate to there
            this._router.navigateByUrl(parentUrl);

            // Throw an error
            return throwError(error);
        }));
    }
}