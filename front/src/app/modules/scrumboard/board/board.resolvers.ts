import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { BoardDetailDto } from 'app/swagger';
import { ScrumboardService } from '../scrumboard.service';

@Injectable({
    providedIn: 'root'
})
export class BoardResolver 
{
    constructor(private _router: Router, private _scrumboardService: ScrumboardService) {
    }
    
    /**
     * Resolver
     *
     * @param route
     * @param state
     */
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<BoardDetailDto> {
        return this._scrumboardService.getBoard(+route.paramMap.get('boardId'))
            .pipe(
                // Error here means the requested task is not available
                catchError((error) => {

                    // Log the error
                    console.error(error);

                    // Get the parent url
                    const parentUrl = state.url.split('/').slice(0, -1).join('/');

                    // Navigate to there
                    this._router.navigateByUrl(parentUrl);

                    // Throw an error
                    return throwError(error);
                })
            );
    }
}