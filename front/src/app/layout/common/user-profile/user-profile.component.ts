import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { IUser } from 'app/core/auth/models/user.model';
import { AuthService } from 'app/core/auth/services/auth.service';

@Component({
  selector: 'user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
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
