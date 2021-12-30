/**
 * Scrumboard API
 * API for accessing Scrumboard project data.
 *
 * OpenAPI spec version: v1
 * Contact: jboinembaome@gmail.com
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 *//* tslint:disable:no-unused-variable member-ordering */

import { Inject, Injectable, Optional }                      from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams,
         HttpResponse, HttpEvent }                           from '@angular/common/http';
import { CustomHttpUrlEncodingCodec }                        from '../encoder';

import { Observable }                                        from 'rxjs';

import { BoardDetailDto } from '../model/boardDetailDto';
import { BoardDto } from '../model/boardDto';
import { CreateBoardCommand } from '../model/createBoardCommand';
import { CreateBoardCommandResponse } from '../model/createBoardCommandResponse';
import { ProblemDetails } from '../model/problemDetails';
import { UpdateBoardCommand } from '../model/updateBoardCommand';
import { UpdatePinnedBoardCommand } from '../model/updatePinnedBoardCommand';

import { BASE_PATH, COLLECTION_FORMATS }                     from '../variables';
import { Configuration }                                     from '../configuration';


@Injectable()
export class BoardsService {

    protected basePath = '';
    public defaultHeaders = new HttpHeaders();
    public configuration = new Configuration();

    constructor(protected httpClient: HttpClient, @Optional()@Inject(BASE_PATH) basePath: string, @Optional() configuration: Configuration) {
        if (basePath) {
            this.basePath = basePath;
        }
        if (configuration) {
            this.configuration = configuration;
            this.basePath = basePath || configuration.basePath || this.basePath;
        }
    }

    /**
     * @param consumes string[] mime-types
     * @return true: consumes contains 'multipart/form-data', false: otherwise
     */
    private canConsumeForm(consumes: string[]): boolean {
        const form = 'multipart/form-data';
        for (const consume of consumes) {
            if (form === consume) {
                return true;
            }
        }
        return false;
    }


    /**
     * Get the boards of the current user.
     * 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiBoardsGet(observe?: 'body', reportProgress?: boolean): Observable<Array<BoardDto>>;
    public apiBoardsGet(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Array<BoardDto>>>;
    public apiBoardsGet(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Array<BoardDto>>>;
    public apiBoardsGet(observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

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

        return this.httpClient.request<Array<BoardDto>>('get',`${this.basePath}/api/Boards`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Delete a board.
     * 
     * @param id Id of the board.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiBoardsIdDelete(id: number, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public apiBoardsIdDelete(id: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public apiBoardsIdDelete(id: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public apiBoardsIdDelete(id: number, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (id === null || id === undefined) {
            throw new Error('Required parameter id was null or undefined when calling apiBoardsIdDelete.');
        }

        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys && this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.request<any>('delete',`${this.basePath}/api/Boards/${encodeURIComponent(String(id))}`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Get a board by id.
     * 
     * @param id Id of the board.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiBoardsIdGet(id: number, observe?: 'body', reportProgress?: boolean): Observable<BoardDetailDto>;
    public apiBoardsIdGet(id: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<BoardDetailDto>>;
    public apiBoardsIdGet(id: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<BoardDetailDto>>;
    public apiBoardsIdGet(id: number, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (id === null || id === undefined) {
            throw new Error('Required parameter id was null or undefined when calling apiBoardsIdGet.');
        }

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

        return this.httpClient.request<BoardDetailDto>('get',`${this.basePath}/api/Boards/${encodeURIComponent(String(id))}`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Update the pinned board.
     * 
     * @param id Id of the board.
     * @param body Pinned board to be updated.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
     public apiBoardsIdPinnedPut(id: number, body?: UpdatePinnedBoardCommand, observe?: 'body', reportProgress?: boolean): Observable<any>;
     public apiBoardsIdPinnedPut(id: number, body?: UpdatePinnedBoardCommand, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
     public apiBoardsIdPinnedPut(id: number, body?: UpdatePinnedBoardCommand, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
     public apiBoardsIdPinnedPut(id: number, body?: UpdatePinnedBoardCommand, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {
 
         if (id === null || id === undefined) {
             throw new Error('Required parameter id was null or undefined when calling apiBoardsIdPinnedPut.');
         }
 
 
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
             'application/json',
             'text/json',
             'application/_*+json'
         ];
         const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
         if (httpContentTypeSelected != undefined) {
             headers = headers.set('Content-Type', httpContentTypeSelected);
         }
 
         return this.httpClient.request<any>('put',`${this.basePath}/api/Boards/${encodeURIComponent(String(id))}/pinned`,
             {
                 body: body,
                 withCredentials: this.configuration.withCredentials,
                 headers: headers,
                 observe: observe,
                 reportProgress: reportProgress
             }
         );
     } 

    /**
     * Update a board.
     * 
     * @param id Id of the board.
     * @param body Board to be updated.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiBoardsIdPut(id: number, body?: UpdateBoardCommand, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public apiBoardsIdPut(id: number, body?: UpdateBoardCommand, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public apiBoardsIdPut(id: number, body?: UpdateBoardCommand, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public apiBoardsIdPut(id: number, body?: UpdateBoardCommand, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (id === null || id === undefined) {
            throw new Error('Required parameter id was null or undefined when calling apiBoardsIdPut.');
        }


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
            'application/json',
            'text/json',
            'application/_*+json'
        ];
        const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
        if (httpContentTypeSelected != undefined) {
            headers = headers.set('Content-Type', httpContentTypeSelected);
        }

        return this.httpClient.request<any>('put',`${this.basePath}/api/Boards/${encodeURIComponent(String(id))}`,
            {
                body: body,
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Create a board.
     * 
     * @param body New board.
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiBoardsPost(observe?: 'body', reportProgress?: boolean): Observable<CreateBoardCommandResponse>;
    public apiBoardsPost(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<CreateBoardCommandResponse>>;
    public apiBoardsPost(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<CreateBoardCommandResponse>>;
    public apiBoardsPost(observe: any = 'body', reportProgress: boolean = false ): Observable<any> {


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
            'application/json',
            'text/json',
            'application/_*+json'
        ];
        const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
        if (httpContentTypeSelected != undefined) {
            headers = headers.set('Content-Type', httpContentTypeSelected);
        }

        return this.httpClient.request<CreateBoardCommandResponse>('post',`${this.basePath}/api/Boards`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

}