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
          if (!request.url.includes('/login')){
            return this.handle401Error(request, next);
          }
          else {
            return throwError(() => error);
          }
        }
        else {
          return throwError(() => error);
        }
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
    return this.authService.refreshToken().pipe(
      switchMap(() => {
        return next.handle(this.addToken(request))
      }),
      catchError((error) => {
        console.error("Failed to refresh token:", error);
        this.authService.logout(); // Log out user or handle error as needed
        return throwError(() => error);
    })
    )
  }
}
