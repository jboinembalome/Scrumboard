import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, ReplaySubject, of } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { User } from 'src/app/core/user/user.types';

@Injectable({
    providedIn: 'root'
})
export class UserService
{
    private _user: ReplaySubject<User> = new ReplaySubject<User>(1);

    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient)
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Setter & getter for user
     *
     * @param value
     */
    set user(value: User)
    {
        // Store the value
        this._user.next(value);
    }

    get user$(): Observable<User>
    {
        return this._user.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get the current logged in user data
     */
    get(): Observable<User>
    {
        const user: User = {
            id: "1",
            name: "Jimmy Boinembalome",
            email: "jboinembalome@gmail.com",
            avatar: "assets/avatars/jimmy.jpg",
            status: "online",
        };

        return of(user);
    }

    /**
     * Update the user
     *
     * @param user
     */
    update(user: User): Observable<any>
    {
         user = {
            id: "1",
            name: "Jimmy Boinembalome",
            email: "jboinembalome@gmail.com",
            avatar: "assets/avatars/jimmy.jpg",
            status: "online",
        };

        return of(user);
    }
}
