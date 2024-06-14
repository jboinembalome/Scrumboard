import { Component, EventEmitter, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { Observable } from 'rxjs';
import { IUser } from 'app/core/auth/models/user.model';
import { AuthService } from 'app/core/auth/services/auth.service';
import { InitialPipe } from '../../../shared/pipes/initial.pipe';
import { AsyncPipe } from '@angular/common';
import { LoginMenuComponent } from './login-menu/login-menu.component';
import { ToggleComponent } from '../../../shared/components/toggle/toggle.component';
import { MatDivider } from '@angular/material/divider';
import { MatIcon } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconButton } from '@angular/material/button';

@Component({
    selector: 'user-profile',
    templateUrl: './user-profile.component.html',
    styleUrls: ['./user-profile.component.scss'],
    encapsulation: ViewEncapsulation.None,
    standalone: true,
    imports: [
      MatIconButton, 
      MatIcon, 
      MatMenuModule, 
      MatDivider, 
      ToggleComponent, 
      LoginMenuComponent, 
      AsyncPipe, 
      InitialPipe]
})
export class UserProfileComponent implements OnInit {

  isAuthenticated: Observable<boolean>;
  status: string = ""; // ex: online

  checkedDarkMode: boolean = false;

  currentUser: Observable<IUser>;

  @Output() toggleTheme = new EventEmitter<void>();
  @Output() toggleDir = new EventEmitter<void>();

  constructor(private _authService: AuthService) {
  }

  ngOnInit(): void {
    this.isAuthenticated = this._authService.isAuthenticated();
    // TODO: Replace authService by userService
    this.currentUser = this._authService.getUser();
  }

  updateChecked() {
    this.checkedDarkMode = !this.checkedDarkMode;
    this.toggleTheme.emit();
  }
}
