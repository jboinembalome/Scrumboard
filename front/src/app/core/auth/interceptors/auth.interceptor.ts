import { Injectable } from "@angular/core";
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
} from "@angular/common/http";
import { Observable } from "rxjs";
import { AuthService } from "../services/auth.service";
import { switchMap } from "rxjs/operators";

@Injectable({
  providedIn: "root",
})
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return this.authService.isAuthenticated().pipe(
      switchMap((isAuthenticated) => {
        if (isAuthenticated) {
          const token = this.authService.accessToken;

          if (token) {
            request = request.clone({
              setHeaders: {
                Authorization: `Bearer ${token}`,
              },
            });
          }
        }
        
        return next.handle(request);
      })
    );
  }
}
