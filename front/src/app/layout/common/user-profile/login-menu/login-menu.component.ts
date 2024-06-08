import { Component, Input, OnInit } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-login-menu',
    templateUrl: './login-menu.component.html',
    styleUrls: ['./login-menu.component.scss'],
    standalone: true,
    imports: [RouterLink, MatIcon]
})
export class LoginMenuComponent {
  @Input() isAuthenticated: boolean;

  constructor() { 
  }
}
