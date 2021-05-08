import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs/operators';

import { FuseConfigService } from '@fuse/services/config.service';
import { FuseNavigationService } from '@fuse/components/navigation/navigation.service';
import { navigation } from 'app/navigation/navigation';
import { AuthorizeService } from '../../../../api-authorization/authorize.service';


@Component({
    selector     : 'vertical-layout-1',
    templateUrl  : './layout-1.component.html',
    styleUrls    : ['./layout-1.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class VerticalLayout1Component implements OnInit, OnDestroy
{
    fuseConfig: any;
    navigation: any;

    // Private
    private _unsubscribeAll: Subject<any>;

    //Public
    public isAuthenticated: Observable<boolean>;
    public userName: Observable<string>;
    

    /**
     * Constructor
     *
     * @param {FuseConfigService} _fuseConfigService
     * @param {FuseNavigationService} _fuseNavigationService
     * @param {AuthorizeService} _authorizeService
     */
    constructor(
        private _fuseConfigService: FuseConfigService,
        private _fuseNavigationService: FuseNavigationService,
        private _authorizeService: AuthorizeService
    )
    {
        // Set the defaults
        this.navigation = navigation;

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
        // Subscribe to config changes
        this._fuseConfigService.config
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((config) => {
                this.fuseConfig = config;
        });

        this._authorizeService.isAuthenticated().subscribe(isAuth => this.hideAuthenticationGroup(isAuth));
        this.userName = this._authorizeService.getUser().pipe(map(u => u && u.name));
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

     /**
     * Hide authentication group in the navbar.
     */
    hideAuthenticationGroup(isAuthenticated: boolean): void 
    {
        // Update the authentication menu item
        this._fuseNavigationService.updateNavigationItem('login', {
            hidden: isAuthenticated
        });

        this._fuseNavigationService.updateNavigationItem('register', {
            hidden: isAuthenticated
        });

        this._fuseNavigationService.updateNavigationItem('logout', {
            hidden: !isAuthenticated
        });
    }
}
