import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-login-menu',
  templateUrl: './login-menu.component.html',
  styleUrls: ['./login-menu.component.scss']
})
export class LoginMenuComponent implements OnInit {
  @Input() isAuthenticated: boolean;

  constructor() { }

  ngOnInit() {
  }
}
