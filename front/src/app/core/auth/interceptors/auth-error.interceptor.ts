import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { catchError, filter, switchMap, take } from 'rxjs/operators';

@Injectable()
export class AuthErrorInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject = new BehaviorSubject<string>(null);
  
  constructor(private authService: AuthService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler) : Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError(error => {
        if (error instanceof HttpErrorResponse && error.status === 401) { 
          // Handle 401 error except for login page
          if (!request.url.includes('/login')){
            return this.handle401Error(request, next);
          }
        }

        return throwError(() => error);
      })
    );
  }

  
  private handle401Error(
    request: HttpRequest<any>,
    next: HttpHandler
  ) : Observable<HttpEvent<any>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      return this.authService.refreshToken().pipe(
        switchMap((x) => {
          this.isRefreshing = false;
          this.refreshTokenSubject.next(x.accessToken);

          return next.handle(this.addTokenFromLocalStorage(request))
        }),
        catchError((error) => {
          console.error("Failed to refresh token:", error);

          this.authService.logout();

          location.reload();

          return throwError(() => error);          
      }));
    }
    
    return this.refreshTokenSubject.pipe(
      filter(token => token !== null),
      take(1),
      switchMap(token => {
        return next.handle(this.addToken(request, token));
      }));
  }

  private addTokenFromLocalStorage(request: HttpRequest<any>): HttpRequest<any> {
    const token = localStorage.getItem('accessToken');

    if (token) {
      return request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    return request;
  }

  private addToken(request: HttpRequest<any>, token: string): HttpRequest<any> {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }
}
