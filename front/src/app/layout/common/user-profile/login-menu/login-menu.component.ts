import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-login-menu',
  templateUrl: './login-menu.component.html',
  styleUrls: ['./login-menu.component.scss']
})
export class LoginMenuComponent {
  @Input() isAuthenticated: boolean;

  constructor() { 
  }
}
