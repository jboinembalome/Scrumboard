import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { take } from 'rxjs/operators';


// The main responsibility of this component is to handle the user's logout process.
// This is the starting point for the logout process, which is usually initiated when a
// user clicks on the logout button on the LoginMenu component.
@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.scss']
})
export class LogoutComponent implements OnInit {
  public message = new BehaviorSubject<string>(null);

  constructor(
    private authService: AuthService,
    private router: Router) { }

  async ngOnInit() {
    await this.logout();
  }

  private async logout(): Promise<void> {
    const isauthenticated = await this.authService.isAuthenticated().pipe(
      take(1)
    ).toPromise();
    
    if (isauthenticated) {
      this.authService.logout();

      this.router.navigate(['home']);
    } else {
      this.message.next('You successfully logged out!');
    }
  }
}
