import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { catchError, switchMap } from 'rxjs/operators';

@Injectable()
export class AuthErrorInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler) : Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError(error => {
        if (error instanceof HttpErrorResponse && error.status === 401) {
          // In case of refresh token expired 
          if (request.url.includes('account/refresh')){
            console.error("Failed to refresh token:", error);

            this.authService.logout();

            location.reload();

            return throwError(() => error);
          }

          // Handle 401 error for other urls except login page
          if (!request.url.includes('/login')){
            return this.handle401Error(request, next);
          }
        }

        return throwError(() => error);
      })
    );
  }

  private addToken(request: HttpRequest<any>): HttpRequest<any> {
    const accessToken = localStorage.getItem('accessToken');
    if (accessToken) {
      return request.clone({
        setHeaders: {
          Authorization: `Bearer ${accessToken}`
        }
      });
    }
    return request;
  }

  private handle401Error(
    request: HttpRequest<any>,
    next: HttpHandler
  ) : Observable<HttpEvent<any>> {
    // Little bit tricky here: 
    // - authService.refreshToken() calls the API  to refresh the token
    // - We enter again into AuthErrorInterceptor...
    // The question: how can we catch the error?
    return this.authService.refreshToken().pipe(
      switchMap(() => {
        return next.handle(this.addToken(request))
      }),
      catchError((error) => {
        console.error("Failed to refresh token:", error);

        this.authService.logout();
        
        return throwError(() => error);
    })
    )
  }
}
