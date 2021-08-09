import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from 'src/app/core/auth/services/auth.service';

@Component({
  selector: 'user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {

  isAuthenticated: Observable<boolean>;
  avatar: string = ""; // ex: assets/images/avatars/jimmy.jpg
  status: string = ""; // ex: online

  userName: Observable<string>;
  
  @Output() toggleTheme = new EventEmitter<void>();
  @Output() toggleDir = new EventEmitter<void>();
  
  constructor(private _authService: AuthService) { 
  }

  ngOnInit(): void {
    this.isAuthenticated = this._authService.isAuthenticated();
    this.userName = this._authService.getUser().pipe(map(u => u && u.name));
  }
}
