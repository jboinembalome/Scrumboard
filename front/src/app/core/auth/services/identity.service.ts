/**
 * Identity Server API
 * API for accessing Identity Server data.
 *//* tslint:disable:no-unused-variable member-ordering */

import { Inject, Injectable, Optional } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpEvent } from '@angular/common/http';

import { Observable } from 'rxjs';

import { IUser } from 'app/core/auth/models/user.model';

import { BASE_PATH } from '../../../swagger/variables';
import { Configuration } from '../../../swagger/configuration';


@Injectable()
export class IdentityService {

    protected basePath = '/';
    public defaultHeaders = new HttpHeaders();
    public configuration = new Configuration();

    constructor(protected httpClient: HttpClient, @Optional() @Inject(BASE_PATH) basePath: string, @Optional() configuration: Configuration) {
        if (basePath) {
            this.basePath = basePath;
        }
        if (configuration) {
            this.configuration = configuration;
            this.basePath = basePath || configuration.basePath || this.basePath;
        }
    }
    /**
     * Get identity information about the current user. (with the current token)
     * 
     */
    public apiUserInfoGet(observe?: 'body', reportProgress?: boolean): Observable<IUser>;
    public apiUserInfoGet(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<IUser>>;
    public apiUserInfoGet(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<IUser>>;
    public apiUserInfoGet(observe: any = 'body', reportProgress: boolean = false): Observable<any> {

        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'application/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        // TODO: Create an endpoint to get user info
        return this.httpClient.request<IUser>('get', `${this.basePath}/api/account/manage/info`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }
}
