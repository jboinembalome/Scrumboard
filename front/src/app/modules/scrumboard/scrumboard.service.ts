import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import {take, tap } from 'rxjs/operators';

import { BoardDetailDto, BoardsService, CardDetailDto, CardsService } from 'app/swagger';

/** Storage service for data like board, labels, members etc... */
@Injectable({
    providedIn: 'root'
})
export class ScrumboardService
{
    private _board: BehaviorSubject<BoardDetailDto | null>;
    private _card: BehaviorSubject<CardDetailDto | null>;

    constructor(
        private _boardsService: BoardsService,
        private _cardsService: CardsService) {
        // Set the private defaults
        this._board = new BehaviorSubject(null);
        this._card = new BehaviorSubject(null);
    }

    get board$(): Observable<BoardDetailDto>
    {
        return this._board.asObservable();
    }

    get card$(): Observable<CardDetailDto>
    {
        return this._card.asObservable();
    }

    getBoard(id: number): Observable<BoardDetailDto>
    {
        return this._boardsService.apiBoardsIdGet(id).pipe(
            tap(board => this._board.next(board))
        );
    }

    getCard(id: number): Observable<CardDetailDto>
    {
        return this._cardsService.apiCardsIdGet(id).pipe(
            tap(card => this._card.next(card))
        );
    }
}
