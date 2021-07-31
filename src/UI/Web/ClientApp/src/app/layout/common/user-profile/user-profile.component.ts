import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from 'src/app/core/auth/services/auth.service';

@Component({
  selector: 'user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {

  public isAuthenticated: Observable<boolean>;
  public avatar: string = ""; // ex: assets/images/avatars/jimmy.jpg
  public status: string = ""; // ex: online

  public userName: Observable<string>;
  
  @Output() toggleTheme = new EventEmitter<void>();
  @Output() toggleDir = new EventEmitter<void>();
  
  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.userName = this.authService.getUser().pipe(map(u => u && u.name));
  }
}
