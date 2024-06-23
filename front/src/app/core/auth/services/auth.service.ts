import { Inject, Injectable, Optional } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { LoginRequest } from "../models/login-request.model";
import { LoginResponse } from "../models/login-response.model";
import { Observable, map, of, throwError } from "rxjs";
import { BASE_PATH } from "../../../swagger/variables";
import { Configuration } from "app/swagger";
import { IUser } from "../models/user.model";
import { IdentityService } from "./identity.service";

@Injectable({
  providedIn: "root",
})
export class AuthService {
  private _authenticated: boolean = false;

  protected basePath = "/";
  public defaultHeaders = new HttpHeaders();
  public configuration = new Configuration();

  constructor(
    private _identityService: IdentityService,
    private _httpClient: HttpClient,
    @Optional() @Inject(BASE_PATH) basePath: string,
    @Optional() configuration: Configuration
  ) {
    if (basePath) {
      this.basePath = basePath;
    }

    if (configuration) {
      this.configuration = configuration;
      this.basePath = basePath || configuration.basePath || this.basePath;
    }
  }

  set accessToken(token: string) {
    localStorage.setItem("accessToken", token);
  }

  get accessToken(): string {
    return localStorage.getItem("accessToken") ?? "";
  }

  public login(credentials: LoginRequest): Observable<LoginResponse> {
    if (this._authenticated) {
      return throwError(() => "User is already logged in.");
    }

    return this._httpClient
      .post<LoginResponse>(`${this.basePath}/api/account/login`, credentials)
      .pipe(
        map((response) => {
          // Store the access token in the local storage
          this.accessToken = response.accessToken;

          // Set the authenticated flag to true
          this._authenticated = true;

          this.storeToken(response);

          return response;
        })
      );
  }

  public refreshToken(): Observable<LoginResponse> {
    const refreshToken = this.getRefreshTokenFromCookie();

    return this._httpClient
      .post<LoginResponse>(`${this.basePath}/api/account/refresh`, {
        refreshToken,
      })
      .pipe(
        map((response) => {
          this.storeToken(response);

          return response;
        })
      );
  }

  public logout(): Observable<boolean> {
    this.removeToken();

    this._authenticated = false;

    return of(true);
  }

  public isAuthenticated(): Observable<boolean> {
    if (this._authenticated) {
      return of(true);
    }

    // Check the access token availability
    if (this.accessToken) {
      return of(true);
    }

    return of(false);
  }

  /**
   * Get the User object for the currently authenticated user.
   */
  public getUser(): Observable<IUser | null> {
    return this._identityService.apiUserInfoGet();
  }

  private getRefreshTokenFromCookie(): string | null {
    const cookieString = document.cookie;
    const cookieArray = cookieString.split("; ");

    for (const cookie of cookieArray) {
      const [name, value] = cookie.split("=");

      if (name == "refreshToken") {
        return value;
      }
    }

    return null;
  }

  private storeToken(response: LoginResponse): void {
    localStorage.setItem("accessToken", response.accessToken);
    document.cookie = `refreshToken=${response.refreshToken};`;
  }

  private removeToken(): void {
    localStorage.removeItem("accessToken");
    document.cookie = 'refreshToken=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/;';
  }
}
