import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import {tap } from 'rxjs/operators';

import { BoardDetailDto, BoardsService } from 'src/app/swagger';

/** Storage service for data like board, labels, members etc... */
@Injectable({
    providedIn: 'root'
})
export class ScrumboardService
{
    private _board: BehaviorSubject<BoardDetailDto | null>;

    constructor(private _boardsService: BoardsService)
    {
        // Set the private defaults
        this._board = new BehaviorSubject(null);
    }

    get board$(): Observable<BoardDetailDto>
    {
        return this._board.asObservable();
    }

    getBoard(id: number): Observable<BoardDetailDto>
    {
        return this._boardsService.apiBoardsIdGet(id).pipe(
            tap(board => this._board.next(board))
        );
    }
}
