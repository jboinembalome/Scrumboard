import { Component } from '@angular/core';
import { SidenavComponent } from './sidenav/sidenav.component';

@Component({
    selector: 'layout',
    templateUrl: './layout.component.html',
    styleUrls: ['./layout.component.scss'],
    standalone: true,
    imports: [SidenavComponent]
})
export class LayoutComponent {

  constructor() { 
  }
}
